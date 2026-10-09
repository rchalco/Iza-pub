import { Component, OnInit } from '@angular/core';
import { Platform } from '@ionic/angular';
import { Router } from '@angular/router';
import { DataDocumento } from 'src/app/interfaces/general/documento';
import { PrinterHelper } from 'src/app/helpers/printer.helper';
import { DocumentoService } from 'src/app/services/documento.service';
import { VentaService } from 'src/app/services/venta.service';
import { environment } from 'src/environments/environment';

/** Ventas de una barra, agrupadas para la impresion en ticket. */
interface GrupoReporte {
  barra: string;
  filas: { nombre: string; cantidad: number; monto: number }[];
  cantidad: number;
  monto: number;
}

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

  gruposProducto: GrupoReporte[] = [];
  gruposMenu: GrupoReporte[] = [];
  totalCantidadProducto: number = 0;

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
    this.gruposProducto = this.agruparPorBarra(this.listVentasXDiaFiltrada);
    this.gruposMenu = this.agruparPorBarra(this.listVentasXDiaXMenuFiltrada);
  }

  calcularTotales() {
    this.totalCantidad = this.listVentasXDiaXMenuFiltrada.reduce((sum, x) => sum + (Number(x.cantidad) || 0), 0);
    this.totalMonto = this.listVentasXDiaXMenuFiltrada.reduce((sum, x) => sum + (Number(x.monto) || 0), 0);
    this.totalCantidadProducto = this.listVentasXDiaFiltrada.reduce((sum, x) => sum + (Number(x.cantidad) || 0), 0);
  }

  /** Agrupa por barra para que el ticket de 80mm no repita la barra en cada fila. */
  private agruparPorBarra(items: any[]): GrupoReporte[] {
    const grupos = new Map<string, GrupoReporte>();
    items.forEach((item) => {
      const barra = item.descripcion || 'Sin barra';
      if (!grupos.has(barra)) {
        grupos.set(barra, { barra, filas: [], cantidad: 0, monto: 0 });
      }
      const grupo = grupos.get(barra);
      const cantidad = Number(item.cantidad) || 0;
      const monto = Number(item.monto) || 0;
      grupo.filas.push({ nombre: item.menu, cantidad, monto });
      grupo.cantidad += cantidad;
      grupo.monto += monto;
    });
    return Array.from(grupos.values()).sort((a, b) => a.barra.localeCompare(b.barra));
  }

  cambiarFiltro() {
    this.aplicarFiltro();
  }

  imprimir() {
    if (!this.tieneDatosParaImprimir()) {
      this.ventaService.showMessageWarning('No hay datos para imprimir en el reporte actual.');
      return;
    }

    const doc = this.construirDocumentoReporte();

    // En escritorio se abre el mismo PDF de 80mm que va a la termica. window.print() sobre
    // la vista de Ionic sale en blanco: ion-app/ion-content no se paginan al imprimir.
    if (!(this.platform.is('android') || this.platform.is('ios'))) {
      this.documentoService.generarDocumento(doc);
      return;
    }

    this.imprimirReporteMovil(doc);
  }

  private tieneDatosParaImprimir(): boolean {
    if (this.segmento === 'producto') {
      return this.listVentasXDiaFiltrada.length > 0;
    }

    return this.listVentasXDiaXMenuFiltrada.length > 0;
  }

  private construirDocumentoReporte(): DataDocumento {
    const porProducto = this.segmento === 'producto';
    const grupos = porProducto ? this.gruposProducto : this.gruposMenu;
    const separarGrupos = grupos.length > 1;

    const doc = new DataDocumento();
    doc.isTableDocument = true;
    doc.isThermalTable = true;
    doc.titulo = [
      'VENTAS POR FECHA DE PROCESO',
      `Fecha: ${this.fechaProcesoCorta()}`,
      `Barra: ${this.filtroDescripcion || 'Todas'}`,
      porProducto ? 'Detalle por producto' : 'Detalle por menu',
    ].join('\n');
    doc.titulosTabla = porProducto ? ['Producto', 'Cant.'] : ['Menu', 'Cant.', 'Monto'];
    doc.anchosColumnas = porProducto ? [4, 1] : [5, 2, 3];
    doc.contenido = [];

    // Convenciones del backend: linea sin '|' = titulo de grupo; '*' al inicio = fila en negrita.
    grupos.forEach((grupo) => {
      if (separarGrupos) {
        doc.contenido.push(this.textoCelda(grupo.barra));
      }
      grupo.filas.forEach((fila) => {
        doc.contenido.push(
          porProducto
            ? `${this.textoCelda(fila.nombre)}|${this.formatoCantidad(fila.cantidad)}`
            : `${this.textoCelda(fila.nombre)}|${this.formatoCantidad(fila.cantidad)}|${this.formatoMonto(fila.monto)}`,
        );
      });
      if (separarGrupos) {
        doc.contenido.push(
          porProducto
            ? `*Subtotal|${this.formatoCantidad(grupo.cantidad)}`
            : `*Subtotal|${this.formatoCantidad(grupo.cantidad)}|${this.formatoMonto(grupo.monto)}`,
        );
      }
    });

    doc.contenido.push(
      porProducto
        ? `*TOTAL|${this.formatoCantidad(this.totalCantidadProducto)}`
        : `*TOTAL|${this.formatoCantidad(this.totalCantidad)}|${this.formatoMonto(this.totalMonto)}`,
    );

    doc.pie = `Impreso: ${new Date().toLocaleString('es-BO')} - SISTEMA VENTAS`;
    return doc;
  }

  /** Evita que un nombre con '|' o '*' al inicio rompa el formato de filas del backend. */
  private textoCelda(valor: string): string {
    return String(valor ?? '').replace(/\|/g, '/').replace(/^\*+/, '').trim() || '-';
  }

  /** Numeros en formato en-US: es el que el backend reconoce para alinearlos a la derecha. */
  private formatoCantidad(valor: number): string {
    return valor.toLocaleString('en-US', { maximumFractionDigits: 2 });
  }

  private formatoMonto(valor: number): string {
    return valor.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  private fechaProcesoCorta(): string {
    const fecha = this.listFechasProceso.find(x => x.idFechaProceso === this.idFechaProceso);
    if (!fecha || !fecha.fechaDeProceso) {
      return String(this.idFechaProceso);
    }
    return new Date(fecha.fechaDeProceso).toLocaleDateString('es-BO', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
    });
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
