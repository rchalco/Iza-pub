import { Component, OnInit } from '@angular/core';
import { Platform } from '@ionic/angular';
import { Router } from '@angular/router';
import { DataDocumento } from 'src/app/interfaces/general/documento';
import { PrinterHelper } from 'src/app/helpers/printer.helper';
import { DocumentoService } from 'src/app/services/documento.service';
import { VentaService } from 'src/app/services/venta.service';
import { environment } from 'src/environments/environment';

@Component({
  standalone: false,
  selector: 'app-reporte-ventas-proceso',
  templateUrl: './reporte-ventas-proceso.page.html',
  styleUrls: ['./reporte-ventas-proceso.page.scss'],
})
export class ReporteVentasProcesoPage implements OnInit {

  idFechaProceso: number = 0;
  filtroDescripcion: string = '';

  listFechasProceso: any[] = [];
  listBarras: string[] = [];
  listVentasXDia: any[] = [];
  listVentasXDiaXMenu: any[] = [];

  listVentasXDiaFiltrada: any[] = [];
  listVentasXDiaXMenuFiltrada: any[] = [];

  segmento: string = 'producto';
  preferredPrinterName: string = '';
  preferredPrinterAddress: string | null = null;

  totalCantidad: number = 0;
  totalMonto: number = 0;

  constructor(
    private ventaService: VentaService,
    private documentoService: DocumentoService,
    private platform: Platform,
    private router: Router,
  ) { }

  ngOnInit() {
    this.actualizarImpresoraPreferida();
    this.cargarFechasProceso();
  }

  actualizarImpresoraPreferida() {
    const preferredPrinter = PrinterHelper.getPreferredPrinter();
    this.preferredPrinterAddress = preferredPrinter.address;
    this.preferredPrinterName = preferredPrinter.name || preferredPrinter.address || '';
  }

  cargarFechasProceso() {
    this.ventaService.obtenerUltimasFechasProceso().then(obs => obs.subscribe(resul => {
      if (resul.state !== 1) {
        this.ventaService.showMessageError(resul.message);
        return;
      }
      this.listFechasProceso = resul.listEntities || [];
      if (this.listFechasProceso.length > 0) {
        const fechaActual = this.listFechasProceso.find(x => x.idFechaProceso === environment.idFechaProceso);
        this.idFechaProceso = fechaActual ? fechaActual.idFechaProceso : this.listFechasProceso[0].idFechaProceso;
        this.cargarReportes();
      }
    }));
  }

  cargarReportes() {
    if (!this.idFechaProceso || this.idFechaProceso <= 0) return;

    this.ventaService.detalleVentasXDiaFechaProceso(this.idFechaProceso).then(obs => obs.subscribe(resul => {
      if (resul.state !== 1) {
        this.ventaService.showMessageError(resul.message);
        return;
      }
      this.listVentasXDia = resul.listEntities || [];
      this.construirListaBarras();
      this.aplicarFiltro();
    }));

    this.ventaService.detalleVentasXDiaXMenuFechaProceso(this.idFechaProceso).then(obs => obs.subscribe(resul => {
      if (resul.state !== 1) {
        this.ventaService.showMessageError(resul.message);
        return;
      }
      this.listVentasXDiaXMenu = resul.listEntities || [];
      this.construirListaBarras();
      this.aplicarFiltro();
    }));
  }

  construirListaBarras() {
    const barrasSet = new Set<string>();
    this.listVentasXDia.forEach(x => { if (x.descripcion) barrasSet.add(x.descripcion); });
    this.listVentasXDiaXMenu.forEach(x => { if (x.descripcion) barrasSet.add(x.descripcion); });
    this.listBarras = Array.from(barrasSet).sort();
  }

  aplicarFiltro() {
    this.listVentasXDiaFiltrada = this.listVentasXDia.filter(x =>
      !this.filtroDescripcion || x.descripcion === this.filtroDescripcion
    );

    this.listVentasXDiaXMenuFiltrada = this.listVentasXDiaXMenu.filter(x =>
      !this.filtroDescripcion || x.descripcion === this.filtroDescripcion
    );

    this.calcularTotales();
  }

  calcularTotales() {
    this.totalCantidad = this.listVentasXDiaXMenuFiltrada.reduce((sum, x) => sum + (Number(x.cantidad) || 0), 0);
    this.totalMonto = this.listVentasXDiaXMenuFiltrada.reduce((sum, x) => sum + (Number(x.monto) || 0), 0);
  }

  cambiarFiltro() {
    this.aplicarFiltro();
  }

  imprimir() {
    if (!this.tieneDatosParaImprimir()) {
      this.ventaService.showMessageWarning('No hay datos para imprimir en el reporte actual.');
      return;
    }

    if (!(this.platform.is('android') || this.platform.is('ios'))) {
      window.print();
      return;
    }

    const doc = this.construirDocumentoReporte();
    this.imprimirReporteMovil(doc);
  }

  private tieneDatosParaImprimir(): boolean {
    if (this.segmento === 'producto') {
      return this.listVentasXDiaFiltrada.length > 0;
    }

    return this.listVentasXDiaXMenuFiltrada.length > 0;
  }

  private construirDocumentoReporte(): DataDocumento {
    const doc = new DataDocumento();
    doc.isTableDocument = true;
    doc.titulo = 'REPORTE VENTAS POR FECHA PROCESO';
    doc.titulo = `${doc.titulo}\nID Fecha Proceso: ${this.idFechaProceso}`;

    if (this.filtroDescripcion) {
      doc.titulo = `${doc.titulo}\nBarra/Almacen: ${this.filtroDescripcion}`;
    }

    doc.titulosTabla = [];
    doc.contenido = [];

    if (this.segmento === 'producto') {
      doc.titulosTabla.push('Fecha');
      doc.titulosTabla.push('Barra');
      doc.titulosTabla.push('Producto');
      doc.titulosTabla.push('Cantidad');

      this.listVentasXDiaFiltrada.forEach((item) => {
        doc.contenido.push(
          `${item.fechaProceso}|${item.descripcion}|${item.menu}|${Number(item.cantidad) || 0}`
        );
      });
    } else {
      doc.titulosTabla.push('Fecha');
      doc.titulosTabla.push('Barra');
      doc.titulosTabla.push('Menu');
      doc.titulosTabla.push('Cantidad');
      doc.titulosTabla.push('Monto');

      this.listVentasXDiaXMenuFiltrada.forEach((item) => {
        doc.contenido.push(
          `${item.fechaProceso}|${item.descripcion}|${item.menu}|${Number(item.cantidad) || 0}|${Number(item.monto) || 0}`
        );
      });

      doc.contenido.push(`|||${this.totalCantidad}|${this.totalMonto}`);
    }

    doc.pie = 'Reporte generado por SISTEMA VENTAS.';
    return doc;
  }

  private imprimirReporteMovil(doc: DataDocumento): void {
    this.documentoService.generarDocumentoPartial(doc).subscribe(async (resul) => {
      const base64String = btoa(String.fromCharCode(...new Uint8Array(resul)));
      this.actualizarImpresoraPreferida();

      if (!this.preferredPrinterAddress) {
        PrinterHelper.savePendingSalePrint(base64String);
        this.ventaService.showMessageWarning(
          'Configura una impresora para continuar con la impresion.',
        );
        this.router.navigate(['/config-printer']);
        return;
      }

      try {
        await PrinterHelper.printPdfBase64(base64String, this.preferredPrinterAddress);
      } catch (error: unknown) {
        console.error('Error al imprimir reporte de ventas por proceso:', error);
        PrinterHelper.savePendingSalePrint(base64String);
        this.ventaService.showMessageWarning(
          'Impresora desconectada apague y prenda su bluetooth nuevamente\n\nNo se pudo imprimir. Verifica la impresora y vuelve a configurar.',
        );
        this.router.navigate(['/config-printer']);
      }
    });
  }

  cambiarSegmento(event: any) {
    this.segmento = event.detail.value;
  }

  fechaSeleccionada() {
    this.filtroDescripcion = '';
    this.cargarReportes();
  }

  fechaSeleccionadaTexto(): string {
    const fecha = this.listFechasProceso.find(x => x.idFechaProceso === this.idFechaProceso);
    if (!fecha || !fecha.fechaDeProceso) {
      return '';
    }
    const fechaObj = new Date(fecha.fechaDeProceso);
    return fechaObj.toLocaleDateString('es-BO', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    });
  }
}
