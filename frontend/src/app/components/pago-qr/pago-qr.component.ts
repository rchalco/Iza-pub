/* eslint-disable @typescript-eslint/member-ordering */
import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { Subscription } from 'rxjs';

import { QrPagadoDTO } from 'src/app/interfaces/pagos/QrPagado';
import { QrTransaccionDTO } from 'src/app/interfaces/pagos/QrTransaccion';
import { PagosQrService } from 'src/app/services/pagos-qr.service';

/** Segundos entre una verificacion del cobro y la siguiente. */
const SEGUNDOS_CICLO = 60;

@Component({
  standalone: false,
  selector: 'app-pago-qr',
  templateUrl: './pago-qr.component.html',
  styleUrls: ['./pago-qr.component.scss'],
})
export class PagoQrComponent implements OnInit, OnDestroy {
  /** Importe exacto del QR. Viaja fijo en el codigo: el pagador no puede cambiarlo. */
  @Input() montoEsperado = 0;

  /** Glosa que ve el pagador. Si es null el backend arma una con la sucursal y la fecha. */
  @Input() descripcion: string = null;

  /** Sucursal que recauda. Si es null el backend usa la configurada. */
  @Input() codigoSucursal: string = null;

  /** El QR fue cobrado. Lleva el movimiento acreditado por el banco. */
  @Output() public eventPagoConfirmado: EventEmitter<QrPagadoDTO> =
    new EventEmitter<QrPagadoDTO>();

  @Output() public eventCancelar: EventEmitter<any> = new EventEmitter<any>();

  // ── QR de la venta ────────────────────────────────────────
  qr: QrTransaccionDTO = null;
  generandoQr = false;
  errorQr = '';

  // ── Ciclo de verificacion ─────────────────────────────────
  segundosRestantes = SEGUNDOS_CICLO;
  verificando = false;
  mensajeEstado = 'Esperando el pago del cliente...';
  errorVerificacion = '';
  pagoConfirmado: QrPagadoDTO = null;

  private temporizador: any = null;
  private suscripcion: Subscription = null;
  private destruido = false;

  constructor(private pagosQrService: PagosQrService) {}

  // ══════════════════════════════════════════════════════════
  //  Lifecycle
  // ══════════════════════════════════════════════════════════

  ngOnInit(): void {
    this.generarQr();
  }

  ngOnDestroy(): void {
    // Sin esto el sondeo sobrevive a la pantalla y sigue golpeando al banco.
    this.destruido = true;
    this.detenerTemporizador();
    this.suscripcion?.unsubscribe();
  }

  // ══════════════════════════════════════════════════════════
  //  Emision del QR
  // ══════════════════════════════════════════════════════════

  /**
   * Emite el QR de esta venta. Solo se reintenta cuando la emision anterior fallo: si ya
   * hay un codigo vivo, emitir otro dejaria dos QR cobrables por el mismo importe.
   */
  generarQr(): void {
    if (this.generandoQr || this.qr) {
      return;
    }

    if (!this.montoEsperado || this.montoEsperado <= 0) {
      this.errorQr = 'El monto de la venta debe ser mayor a cero para generar el QR.';
      return;
    }

    this.generandoQr = true;
    this.errorQr = '';

    this.pagosQrService
      .generaQrTransaccion(
        this.montoEsperado,
        this.descripcion,
        this.codigoSucursal,
        true,
      )
      .subscribe({
        next: (resul) => {
          this.generandoQr = false;

          if (resul.state !== 1 || !resul.data) {
            this.errorQr = resul.message || 'No se pudo generar el QR de cobro.';
            return;
          }

          this.qr = resul.data as QrTransaccionDTO;
          this.mensajeEstado = 'Esperando el pago del cliente...';
          this.iniciarTemporizador();
        },
        error: (error) => {
          this.generandoQr = false;
          this.errorQr = error?.message || 'No se pudo generar el QR de cobro.';
        },
      });
  }

  /** Imagen lista para el `src`. El banco puede o no mandar el prefijo `data:`. */
  get qrImagenSrc(): string {
    if (!this.qr?.qrImageBase64) {
      return '';
    }
    return this.qr.qrImageBase64.startsWith('data:')
      ? this.qr.qrImageBase64
      : 'data:image/png;base64,' + this.qr.qrImageBase64;
  }

  // ══════════════════════════════════════════════════════════
  //  Verificacion del cobro
  // ══════════════════════════════════════════════════════════

  /** Fuerza la verificacion sin esperar a que termine el temporizador. */
  verificarAhora(): void {
    if (this.verificando || this.pagoConfirmado || !this.qr) {
      return;
    }
    this.detenerTemporizador();
    this.verificarPago();
  }

  cancelar(): void {
    this.detenerTemporizador();
    this.eventCancelar.emit();
  }

  // ══════════════════════════════════════════════════════════
  //  Metodos privados
  // ══════════════════════════════════════════════════════════

  private iniciarTemporizador(): void {
    this.detenerTemporizador();

    if (this.destruido || this.pagoConfirmado || !this.qr) {
      return;
    }

    this.segundosRestantes = SEGUNDOS_CICLO;
    this.temporizador = setInterval(() => {
      this.segundosRestantes--;

      if (this.segundosRestantes <= 0) {
        this.detenerTemporizador();
        this.verificarPago();
      }
    }, 1000);
  }

  private detenerTemporizador(): void {
    if (this.temporizador) {
      clearInterval(this.temporizador);
      this.temporizador = null;
    }
  }

  /**
   * Pregunta al backend si este qrId ya fue cobrado. Como el QR es de un solo uso y de
   * monto fijo, un cobro sobre el es exactamente el de esta venta: no hay que conciliar
   * por importe ni descartar movimientos ajenos.
   */
  private verificarPago(): void {
    if (this.destruido || !this.qr) {
      return;
    }

    this.verificando = true;
    this.errorVerificacion = '';
    this.suscripcion?.unsubscribe();

    this.suscripcion = this.pagosQrService
      .consultaPagoQr(this.qr.qrId, new Date(), false)
      .subscribe({
        next: (resul) => {
          this.verificando = false;

          if (this.destruido) {
            return;
          }

          if (resul.state === 1 && resul.data) {
            this.pagoConfirmado = resul.data as QrPagadoDTO;
            this.detenerTemporizador();
            this.mensajeEstado = 'Pago recibido. Registrando la venta...';
            this.eventPagoConfirmado.emit(this.pagoConfirmado);
            return;
          }

          // state 4 (NoData) es "todavia no pago": el caso normal mientras se espera.
          if (resul.state === 4) {
            this.mensajeEstado = 'Aun no se registra el pago. Se verificara de nuevo.';
          } else {
            this.errorVerificacion =
              resul.message || 'No se pudo verificar el cobro del QR.';
          }

          this.iniciarTemporizador();
        },
        error: (error) => {
          this.verificando = false;

          if (this.destruido) {
            return;
          }

          // Un fallo de red no corta el ciclo: se anota y se reintenta en el turno siguiente.
          this.errorVerificacion =
            error?.message || 'No se pudo verificar el cobro del QR.';
          this.iniciarTemporizador();
        },
      });
  }
}
