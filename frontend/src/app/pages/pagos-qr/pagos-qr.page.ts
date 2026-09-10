import { Component, OnInit } from '@angular/core';

import { QrPagadoDTO } from 'src/app/interfaces/pagos/QrPagado';
import { PagosQrService } from 'src/app/services/pagos-qr.service';

@Component({
  standalone: false,
  selector: 'app-pagos-qr',
  templateUrl: './pagos-qr.page.html',
  styleUrls: ['./pagos-qr.page.scss'],
})
export class PagosQrPage implements OnInit {
  /** El banco reporta por dia, no por rango: la consulta siempre es de una fecha. */
  fecha: Date = new Date();

  pagos: QrPagadoDTO[] = [];
  cargando = false;
  consultado = false;
  mensaje = '';

  constructor(private pagosQrService: PagosQrService) {}

  ngOnInit(): void {
    this.consultar();
  }

  cambiarFecha(fecha: Date): void {
    this.fecha = fecha;
    this.consultar();
  }

  consultar(): void {
    if (this.cargando) {
      return;
    }

    this.cargando = true;
    this.mensaje = '';

    this.pagosQrService.obtieneQrsPagados(this.fecha, true).subscribe({
      next: (resul) => {
        this.cargando = false;
        this.consultado = true;

        // state 4 (NoData) es un dia sin cobros, no un error.
        if (resul.state !== 1 && resul.state !== 4) {
          this.pagos = [];
          this.mensaje = resul.message || 'No se pudo obtener el reporte de cobros.';
          return;
        }

        this.pagos = this.ordenar((resul.listEntities ?? []) as QrPagadoDTO[]);
        if (this.pagos.length === 0) {
          this.mensaje = resul.message || 'No hay cobros por QR en la fecha seleccionada.';
        }
      },
      error: (error) => {
        this.cargando = false;
        this.consultado = true;
        this.pagos = [];
        this.mensaje = error?.message || 'No se pudo obtener el reporte de cobros.';
      },
    });
  }

  /** Total acreditado en la fecha; es el numero que se cruza contra el cierre de caja. */
  get totalCobrado(): number {
    return this.pagos.reduce((suma, pago) => suma + Number(pago.monto ?? 0), 0);
  }

  get moneda(): string {
    return this.pagos.length > 0 ? this.pagos[0].moneda : 'BOB';
  }

  /** Mas recientes primero: el banco no garantiza el orden del reporte. */
  private ordenar(pagos: QrPagadoDTO[]): QrPagadoDTO[] {
    return [...pagos].sort((a, b) =>
      this.instanteDe(b).localeCompare(this.instanteDe(a)),
    );
  }

  private instanteDe(pago: QrPagadoDTO): string {
    return `${pago.fechaPago ?? ''}T${pago.horaPago ?? ''}`;
  }
}
