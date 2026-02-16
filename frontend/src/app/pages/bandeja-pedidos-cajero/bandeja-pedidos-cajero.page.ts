/* eslint-disable @typescript-eslint/naming-convention */
import { Component, OnInit } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import { exportDataGrid } from 'devextreme/excel_exporter';
import notify from 'devextreme/ui/notify';
import { Workbook } from 'exceljs';
import { saveAs } from 'file-saver-es';

import { DetallePedidosDTO } from 'src/app/interfaces/venta/detallePedido';
import { VentaService } from 'src/app/services/venta.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-bandeja-pedidos-cajero',
  templateUrl: './bandeja-pedidos-cajero.page.html',
  styleUrls: ['./bandeja-pedidos-cajero.page.scss'],
})
export class BandejaPedidosCajeroPage implements OnInit {
  // ── Datos de pedidos ──────────────────────────────────────
  listaPedidos: DetallePedidosDTO[] = [];
  listaPedidosFiltrados: DetallePedidosDTO[] = [];
  dataSourcePedidos: DataSource;

  // ── Fechas ────────────────────────────────────────────────
  fechaPedido: Date;
  fechaPedidoFin: Date;
  fechaHoyLabel = '';
  readonly minDate = new Date(2024, 11, 1);

  // ── Usuario ───────────────────────────────────────────────
  usuarioActual = '';

  // ── UI state ──────────────────────────────────────────────
  showDetalle = true;
  popupVisible = false;

  // ── Forma de pago ─────────────────────────────────────────
  idFormaPago = 0;
  idPedidoFormaPago = 0;

  // ── Opciones de botones del popup ─────────────────────────
  closeButtonOptions: Record<string, unknown>;
  aceptarButtonOptions: Record<string, unknown>;

  constructor(private ventaService: VentaService) {
    this.initPopupButtons();
  }

  // ══════════════════════════════════════════════════════════
  //  Lifecycle
  // ══════════════════════════════════════════════════════════

  ngOnInit(): void {
    this.initFechas();
    this.initUsuario();
    this.buscarPedidos();

    // Bindear callbacks para DevExtreme
    this.anularPedido = this.anularPedido.bind(this);
    this.abrirFormaPago = this.abrirFormaPago.bind(this);
  }

  // ══════════════════════════════════════════════════════════
  //  Consultas
  // ══════════════════════════════════════════════════════════

  buscarPedidos(): void {
    this.showDetalle = true;

    this.ventaService
      .detallePedidoPorFormaPago(this.fechaPedido, this.fechaPedidoFin)
      .then((service) => {
        service.subscribe((resul) => {
          this.listaPedidos = resul.listEntities;
          this.aplicarFiltroUsuario();
          this.ventaService.showMessageResponse(resul);
        });
      });
  }

  // ══════════════════════════════════════════════════════════
  //  Acciones sobre pedidos
  // ══════════════════════════════════════════════════════════

  anularPedido(e: any): void {
    const pedido: DetallePedidosDTO = e.row.data;
    this.ventaService.anulaPedido(pedido.idPedMaster).then((service) => {
      service.subscribe((resul) => {
        this.listaPedidos = resul.listEntities;
        this.aplicarFiltroUsuario();
        this.ventaService.showMessageResponse(resul);
      });
    });
  }

  // ══════════════════════════════════════════════════════════
  //  Forma de pago
  // ══════════════════════════════════════════════════════════

  abrirFormaPago(e: any): void {
    if (!e) {
      return;
    }
    const pedido: DetallePedidosDTO = e.row.data;
    this.idPedidoFormaPago = pedido.idPedFormaPago;
    this.popupVisible = true;
  }

  setFormaPago(event: any): void {
    this.idFormaPago = parseInt(event.detail.value, 10);
  }

  cambiarFormaPago(): void {
    this.ventaService
      .actualizaFormaPagoPedido(this.idPedidoFormaPago, this.idFormaPago)
      .then((service) => {
        service.subscribe((resul) => {
          this.listaPedidos = resul.listEntities;
          this.buscarPedidos();
          this.aplicarFiltroUsuario();
        });
      });
  }

  // ══════════════════════════════════════════════════════════
  //  Exportar a Excel
  // ══════════════════════════════════════════════════════════

  onExporting(e: any): void {
    const fecha = new Date(this.fechaPedido);
    const fechaFin = new Date(this.fechaPedidoFin);

    const prefijo = this.formatDateForFile(fecha);
    const sufijo = this.formatDateForFile(fechaFin);

    const workbook = new Workbook();
    const worksheet = workbook.addWorksheet('Pedidos_Cajero');

    exportDataGrid({
      component: e.component,
      worksheet,
      topLeftCell: { row: 4, column: 1 },
    })
      .then(() => {
        // Título principal
        const headerRow = worksheet.getRow(2);
        headerRow.height = 30;
        worksheet.mergeCells(2, 1, 2, 5);
        headerRow.getCell(1).value =
          `DETALLE DE PEDIDOS DEL CAJERO: ${this.usuarioActual}`;
        headerRow.getCell(1).font = { name: 'Segoe UI Light', size: 22 };
        headerRow.getCell(1).alignment = { horizontal: 'center' };

        // Subtítulo con rango de fechas
        const subHeaderRow = worksheet.getRow(3);
        subHeaderRow.height = 30;
        worksheet.mergeCells(3, 1, 3, 5);
        subHeaderRow.getCell(1).value =
          `DESDE ${this.formatDateDisplay(fecha)} HASTA ${this.formatDateDisplay(fechaFin)}`;
        subHeaderRow.getCell(1).font = { name: 'Segoe UI Light', size: 18 };
        subHeaderRow.getCell(1).alignment = { horizontal: 'center' };
      })
      .then(() => {
        workbook.xlsx.writeBuffer().then((buffer) => {
          const fileName = `${prefijo}_${sufijo}_Pedidos_Cajero_${this.usuarioActual}.xlsx`;
          saveAs(
            new Blob([buffer], { type: 'application/octet-stream' }),
            fileName,
          );
        });
      });
  }

  // ══════════════════════════════════════════════════════════
  //  Métodos privados
  // ══════════════════════════════════════════════════════════

  private initFechas(): void {
    const hoy = new Date();
    this.fechaPedido = new Date(
      hoy.getFullYear(),
      hoy.getMonth(),
      hoy.getDate(),
    );
    this.fechaPedidoFin = new Date(
      hoy.getFullYear(),
      hoy.getMonth(),
      hoy.getDate(),
    );
    this.fechaHoyLabel = hoy.toLocaleDateString('es-BO', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    });
  }

  private initUsuario(): void {
    this.usuarioActual = environment.Usuario;
  }

  private initPopupButtons(): void {
    this.closeButtonOptions = {
      text: 'Cerrar',
      stylingMode: 'outlined',
      type: 'normal',
      onClick: () => {
        this.popupVisible = false;
      },
    };

    this.aceptarButtonOptions = {
      icon: 'send',
      stylingMode: 'contained',
      text: 'Cambiar',
      onClick: () => {
        this.cambiarFormaPago();
        notify(
          {
            message: 'Se realizó el cambio de forma de pago',
            position: { my: 'center top', at: 'center top' },
          },
          'success',
          3000,
        );
        this.popupVisible = false;
      },
    };
  }

  /**
   * Filtra la lista de pedidos dejando solo los del usuario logueado
   * y actualiza el DataSource del grid.
   */
  private aplicarFiltroUsuario(): void {
    const usuario = this.usuarioActual?.toUpperCase();
    this.listaPedidosFiltrados = this.listaPedidos.filter(
      (p) => p.usuario?.toUpperCase() === usuario,
    );
    this.dataSourcePedidos = new DataSource(this.listaPedidosFiltrados);
  }

  /** Formatea una fecha como "YYYY/MM/DD" para mostrar. */
  private formatDateDisplay(date: Date): string {
    return `${date.getFullYear()}/${date.getMonth() + 1}/${date.getDate()}`;
  }

  /** Formatea una fecha como "YYYYMMDD" para nombres de archivo. */
  private formatDateForFile(date: Date): string {
    const y = date.getFullYear();
    const m = (date.getMonth() + 1).toString().padStart(2, '0');
    const d = date.getDate().toString().padStart(2, '0');
    return `${y}${m}${d}`;
  }
}
