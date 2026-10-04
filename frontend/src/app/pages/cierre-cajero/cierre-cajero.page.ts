import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertController, Platform } from '@ionic/angular';
import { firstValueFrom } from 'rxjs';
import {
  CierreImpresionHelper,
  CierreImpresionRegistro,
} from 'src/app/helpers/cierre-impresion.helper';
import { PrinterHelper } from 'src/app/helpers/printer.helper';
import { DataDocumento } from 'src/app/interfaces/general/documento';
import { DocumentoService } from 'src/app/services/documento.service';
import { VentaService } from 'src/app/services/venta.service';
import { customFormatter } from 'src/environments/environment';

const ESTADO_RESPUESTA_EXITO = 1;
const MENSAJE_CIERRE_SIN_IMPRESION =
  'La caja fue cerrada correctamente, pero no se imprimio el comprobante. Imprimelo desde el boton "Reimprimir cierres".';

@Component({
  standalone: false,
  selector: 'app-cierre-cajero',
  templateUrl: './cierre-cajero.page.html',
  styleUrls: ['./cierre-cajero.page.scss'],
})
export class CierreCajeroPage implements OnInit {
  formasPagos = [];
  formasPagosTotales = [];
  montoApertura = 0;
  montoTotalVenta = 0;
  cajeros = [];
  currentCajero;
  observaciones = '';
  hasPendingPrint = false;
  preferredPrinterName = '';
  cierresGuardados = 0;
  cierresPendientes = 0;

  constructor(
    private ventaService: VentaService,
    private documentoService: DocumentoService,
    private platform: Platform,
    private router: Router,
    private alertController: AlertController
  ) { }

  ngOnInit() {
    this.actualizarEstadoImpresionPendiente();
    this.obtieneCajeros();
  }

  ionViewWillEnter() {
    this.actualizarEstadoImpresionPendiente();
  }

  actualizarEstadoImpresionPendiente() {
    const pendingPdfBase64 = PrinterHelper.getPendingSalePrint();
    const preferredPrinter = PrinterHelper.getPreferredPrinter();
    this.hasPendingPrint = !!pendingPdfBase64;
    this.preferredPrinterName = preferredPrinter.name || preferredPrinter.address || '';
    this.cierresGuardados = CierreImpresionHelper.listar().length;
    this.cierresPendientes = CierreImpresionHelper.pendientes();
  }

  obtieneCajeros() {


    this.ventaService.obtenerCajeros().then((service) => {
      service.subscribe((resul) => {
        console.log('obtenerCajeros', resul);
        this.cajeros = resul.listEntities;
        this.montoTotalVenta = 0;
        this.montoApertura = 0;
        this.formasPagosTotales = [];
        this.currentCajero = null;
      });
    });
  }

  obtieneArqueo(event) {
    if (!event.detail.value) {
      return;
    }
    this.montoTotalVenta = 0;
    this.montoApertura = 0;
    this.formasPagosTotales = [];
    console.log('obtieneArqueo', event);
    this.currentCajero = event.detail.value;
    this.ventaService
      .obtenerArqueoCajero(this.currentCajero.idOperacionDiariaCaja)
      .then((service) => {
        service.subscribe(async (resul) => {
          this.formasPagos = resul.listEntities;
          console.log('this.formasPagos', this.formasPagos);
          ///TODO llenamos las formas de pagos totales
          this.formasPagos.forEach((x) => {
            this.montoApertura = x.montoApertura;
            if (
              this.formasPagosTotales.filter(
                (yy) => yy.idFormaDePago === x.idFormaDePago
              ).length === 0
            ) {
              const totalFormaPago = {
                idFormaDePago: x.idFormaDePago,
                formaDePago: x.formaDePago,
                totalVendido: 0,
                diferencia: 0,
                totalDeclarado: 0,
                observaciones: ''
              };
              totalFormaPago.totalVendido = this.formasPagos
                .filter((zzz) => zzz.idFormaDePago === x.idFormaDePago)
                .reduce((sum, current) => sum + current.montoCubierto, 0);
              totalFormaPago.observaciones = this.observaciones;
              this.formasPagosTotales.push(totalFormaPago);
              this.montoTotalVenta += totalFormaPago.totalVendido;
            }
          });
        });
      });
  }

  /**
   * El cierre de caja nunca depende de la impresion: primero se confirma el
   * cierre en el backend, luego se intenta imprimir. Si la impresion falla el
   * comprobante queda en el historial para reimprimirlo.
   */
  async realizarCierre() {
    if (!this.currentCajero) {
      this.ventaService.showMessageWarning('Selecciona un cajero para realizar el cierre.');
      return;
    }

    this.formasPagosTotales.forEach(x => {
      x.observaciones = this.observaciones;
    });

    ///TODO el refresco de cajeros limpia la pantalla, por eso se arma el documento antes
    const cajeroCierre = this.currentCajero;
    const documento = this.construirDocumentoCierre(
      cajeroCierre,
      this.montoApertura,
      this.montoTotalVenta,
      this.formasPagosTotales,
      this.observaciones
    );

    let respuesta;
    try {
      const service = await this.ventaService.realizarCierreCaja(
        this.formasPagosTotales,
        this.montoApertura,
        this.montoTotalVenta,
        cajeroCierre.idOperacionDiariaCaja
      );
      respuesta = await firstValueFrom(service);
    } catch (error: unknown) {
      ///TODO el servicio ya notifico el error al usuario
      console.error('Error al realizar el cierre de caja:', error);
      return;
    }

    this.ventaService.showMessageResponse(respuesta);

    if (respuesta?.state !== ESTADO_RESPUESTA_EXITO) {
      return;
    }

    const registro: CierreImpresionRegistro = {
      id: `${cajeroCierre.idOperacionDiariaCaja}-${Date.now()}`,
      idOperacionDiariaCaja: cajeroCierre.idOperacionDiariaCaja,
      cajero: cajeroCierre.usuario || cajeroCierre.nombreCompleto || '',
      fechaCierre: new Date().toISOString(),
      montoTotalCierre: this.montoTotalVenta,
      impreso: false,
      documento,
      pdfBase64: null,
    };
    CierreImpresionHelper.registrar(registro);
    this.actualizarEstadoImpresionPendiente();

    this.obtieneCajeros();

    const impreso = await this.imprimirRegistroCierre(registro, false);

    if (!impreso) {
      this.ventaService.showMessageWarning(MENSAJE_CIERRE_SIN_IMPRESION);
    }
  }

  calcularDiferencia(pFormaPago) {
    pFormaPago.diferencia = pFormaPago.totalVendido - pFormaPago.entregado;
    pFormaPago.totalDeclarado = pFormaPago.entregado;
  }

  /** Lista los cierres guardados para volver a imprimir uno de ellos. */
  async abrirReimpresionCierres(): Promise<void> {
    const cierres = CierreImpresionHelper.listar();
    this.cierresGuardados = cierres.length;
    this.cierresPendientes = cierres.filter((x) => !x.impreso).length;

    if (!cierres.length) {
      this.ventaService.showMessageWarning('No hay cierres registrados para reimprimir.');
      return;
    }

    const alert = await this.alertController.create({
      header: 'Reimprimir cierres',
      inputs: cierres.map((cierre, indice) => ({
        type: 'radio' as const,
        label: this.describirCierre(cierre),
        value: cierre.id,
        checked: indice === 0,
      })),
      buttons: [
        { text: 'Cancelar', role: 'cancel' },
        {
          text: 'Imprimir',
          handler: (idSeleccionado: string) => {
            if (!idSeleccionado) {
              return false;
            }
            this.reimprimirCierre(idSeleccionado);
            return true;
          },
        },
      ],
    });

    await alert.present();
  }

  async reimprimirCierre(id: string): Promise<void> {
    const registro = CierreImpresionHelper.obtener(id);

    if (!registro) {
      this.ventaService.showMessageWarning('El cierre seleccionado ya no esta disponible.');
      this.actualizarEstadoImpresionPendiente();
      return;
    }

    const impreso = await this.imprimirRegistroCierre(registro, true);

    if (impreso) {
      this.ventaService.showMessageSucess('Reimpresion realizada correctamente.');
    }
  }

  private describirCierre(cierre: CierreImpresionRegistro): string {
    const fecha = new Date(cierre.fechaCierre);
    const fechaTexto = isNaN(fecha.getTime())
      ? cierre.fechaCierre
      : `${fecha.toLocaleDateString()} ${fecha.toLocaleTimeString()}`;
    const estado = cierre.impreso ? '' : '[Sin imprimir] ';

    return `${estado}${cierre.cajero} - ${fechaTexto} - ${customFormatter(cierre.montoTotalCierre)}`;
  }

  private construirDocumentoCierre(
    cajero,
    montoApertura: number,
    montoTotalVenta: number,
    formasPagosTotales,
    observaciones: string
  ): DataDocumento {
    console.log('construirDocumentoCierre init');
    const doc = new DataDocumento();
    doc.titulo = '-----------------------------------';
    doc.titulo = doc.titulo + '\n' + 'CIERRE CAJERO';
    //doc.titulo = doc.titulo + '\n' + this.empleadoSeleccionado.nombreCompleto;
    //doc.titulo = doc.titulo + '\n' + this.selectedLugarConsumo.descripcion;
    doc.titulo = doc.titulo + '\n' + '-----------------------------------';

    doc.contenido = new Array();
    doc.contenido.push('Monto Apertura:...Bs.' + montoApertura);
    doc.contenido.push('Total Ventas:.....Bs.' + montoTotalVenta);
    doc.contenido.push(
      'Datos Cajero: ' + cajero.ci + ' ' + cajero.nombreCompleto
    );
    doc.contenido.push('Barra: ' + cajero.barra);
    doc.contenido.push('Observaciones: ' + observaciones);
    doc.contenido.push('=============================');
    doc.contenido.push('...Totales por metodo de pago...');
    doc.contenido.push('=============================');

    formasPagosTotales.forEach((x) => {
      x.diferencia = x.diferencia ? x.diferencia : 0;
      x.entregado = x.entregado ? x.entregado : 0;
      doc.contenido.push('Forma de Pago: .....' + x.formaDePago);
      doc.contenido.push('Total:..........' + customFormatter(x.totalVendido));
      doc.contenido.push('Entregado:......' + customFormatter(x.entregado));
      doc.contenido.push('Diferencia:.....' + customFormatter(x.diferencia));
      doc.contenido.push('*************************\n');
    });

    doc.pie = '\n\n\n\nFirma Cajero';
    doc.pie +=
      '\nCI: ' + cajero.ci + '\nNombre: ' + cajero.nombreCompleto;
    doc.pie += '\n\n\n\nFirma Administrador';
    console.log('Documento', doc);

    return doc;
  }

  /**
   * Imprime (o reimprime) el comprobante de un cierre ya registrado.
   * Devuelve false cuando la impresion no se pudo completar; el cierre
   * permanece en el historial para reintentarlo.
   */
  private async imprimirRegistroCierre(
    registro: CierreImpresionRegistro,
    esReimpresion: boolean
  ): Promise<boolean> {
    let pdfBase64 = registro.pdfBase64;

    if (!pdfBase64) {
      try {
        pdfBase64 = await this.generarPdfCierre(registro.documento);
        CierreImpresionHelper.actualizar(registro.id, { pdfBase64 });
      } catch (error: unknown) {
        console.error('No se pudo generar el comprobante de cierre:', error);
        if (esReimpresion) {
          this.ventaService.showMessageWarning(
            'No se pudo generar el comprobante del cierre. Intenta nuevamente.'
          );
        }
        return false;
      }
    }

    if (!(this.platform.is('android') || this.platform.is('ios'))) {
      this.abrirPdfEnWeb(pdfBase64);
      CierreImpresionHelper.actualizar(registro.id, { impreso: true });
      this.actualizarEstadoImpresionPendiente();
      return true;
    }

    const preferredPrinter = PrinterHelper.getPreferredPrinter().address;

    if (!preferredPrinter) {
      if (esReimpresion) {
        this.ventaService.showMessageWarning(
          'Configura una impresora para continuar con la impresion.'
        );
        this.router.navigate(['/config-printer']);
      }
      return false;
    }

    try {
      await PrinterHelper.printPdfBase64(pdfBase64, preferredPrinter);
      CierreImpresionHelper.actualizar(registro.id, { impreso: true });
      this.actualizarEstadoImpresionPendiente();
      return true;
    } catch (error: unknown) {
      console.error('Error al imprimir el cierre de caja:', error);
      if (esReimpresion) {
        this.ventaService.showMessageWarning(
          'No se pudo imprimir. Verifica la impresora y vuelve a intentar.'
        );
      }
      return false;
    }
  }

  private async generarPdfCierre(documento: DataDocumento): Promise<string> {
    const buffer = await firstValueFrom(
      this.documentoService.generarDocumentoPartial(documento)
    );

    return PrinterHelper.arrayBufferToBase64(buffer as ArrayBuffer);
  }

  private abrirPdfEnWeb(base64: string): void {
    const blob = new Blob([PrinterHelper.base64ToUint8Array(base64)], {
      type: 'application/pdf',
    });
    const url = window.URL.createObjectURL(blob);
    const pdf = window.open(url);

    if (!pdf || pdf.closed || typeof pdf.closed === 'undefined') {
      this.ventaService.showMessageWarning(
        'Deshabilita el bloqueador de ventanas emergentes para ver el comprobante.'
      );
    }
  }

  async reintentarImpresion(): Promise<void> {
    const pendingPdfBase64 = PrinterHelper.getPendingSalePrint();

    if (!pendingPdfBase64) {
      this.hasPendingPrint = false;
      this.ventaService.showMessageWarning('No hay una impresion pendiente para reintentar.');
      return;
    }

    if (!(this.platform.is('android') || this.platform.is('ios'))) {
      this.ventaService.showMessageWarning('El reintento de impresion aplica en dispositivos moviles.');
      return;
    }

    const preferredPrinter = PrinterHelper.getPreferredPrinter().address;
    if (!preferredPrinter) {
      this.ventaService.showMessageWarning('Configura una impresora para reintentar la impresion.');
      this.router.navigate(['/config-printer']);
      return;
    }

    try {
      await PrinterHelper.printPdfBase64(pendingPdfBase64, preferredPrinter);
      PrinterHelper.clearPendingSalePrint();
      this.hasPendingPrint = false;
      this.ventaService.showMessageSucess('Impresion realizada correctamente.');
    } catch (error) {
      console.error('Error al reintentar impresion en cierre-cajero:', error);
      this.ventaService.showMessageWarning('No se pudo reintentar la impresion. Verifica la impresora.');
    }
  }
}
