import { Component, OnInit } from '@angular/core';
import { VentaService } from 'src/app/services/venta.service';
import { exportDataGrid } from 'devextreme/excel_exporter';
import { Workbook } from 'exceljs';
import { saveAs } from 'file-saver-es';
import { ResponseVentasXDiaXMenu } from 'src/app/interfaces/reportes/ResponseVentasXDiaXMenu';
import { DataSource } from 'devextreme/common/data';

@Component({
  selector: 'app-reportes-generales',
  templateUrl: './reportes-generales.page.html',
  styleUrls: ['./reportes-generales.page.scss'],
})
export class ReportesGeneralesPage implements OnInit {


  fechaInicio: Date = new Date();
  fechaFin: Date = new Date();
  now: Date = new Date();

  reporteSeleccionado: number = 1;

  tiposReporte = [
    { id: 1, nombre: 'Ventas Por Dia - Menu' },
    { id: 2, nombre: 'Ventas por Dia' },
    { id: 3, nombre: 'Varios' }
  ];

  tabsReportes: any[] = [];
  tabSeleccionadoIndex = 0;
  tabActivo: any = null;

  ventasPorDiaMenu: ResponseVentasXDiaXMenu[] = [];
  dataSourceVentasPorDia: DataSource = new DataSource(this.ventasPorDiaMenu);

  ventasPorDiaCantidad: any[] = [];
  reporteVarios: any[] = [];

  constructor(private ventaService: VentaService) { }

  ngOnInit() {
    this.reporteSeleccionado = 1;
    this.consultarReportes();
  }

  consultarReportes(): void {
    const reporte = this.tiposReporte.find(x => x.id === this.reporteSeleccionado);

    if (!reporte) return;

    if (!this.tabsReportes.some(x => x.id === reporte.id)) {
      this.tabsReportes = [...this.tabsReportes, reporte];
    }

    this.tabActivo = this.tabsReportes.find(x => x.id === reporte.id);

    this.cargarReporte(reporte.id);
  }

  seleccionarTab(e: any): void {
    this.tabActivo = e.itemData;
  }
  cargarReporte(idReporte: number): void {
    switch (idReporte) {
      case 1:
        this.cargarVentasPorDia();
        break;

      case 2:
        this.cargarVentasPorDiaCantidad();
        break;

      case 3:
        this.cargarVarios();
        break;
    }
  }

  cargarVentasPorDia(): void {
    this.ventasPorDiaMenu = [];
    this.ventaService.detalleVentasXDiaXMenu(this.fechaInicio, this.fechaFin).then((service) => {
      service.subscribe((resul) => {
        this.ventasPorDiaMenu = resul.listEntities;
        this.dataSourceVentasPorDia = new DataSource(this.ventasPorDiaMenu);
        this.ventaService.showMessageResponse(resul);

      });
    });
    // Aquí llamas tu servicio/API
  }

  cargarVentasPorDiaCantidad(): void {
    this.ventasPorDiaCantidad = [];
    // Aquí llamas tu servicio/API
  }

  cargarVarios(): void {
    this.reporteVarios = [];
    // Aquí llamas tu servicio/API
  }

  onExportingVentasPorDiaMenu(e: any) {
    //this.fechaPedido = Date.now();
    var fecha =  new Date(this.fechaInicio);
    var year  = fecha.getFullYear();
    var month = (fecha.getMonth() + 1).toString().padStart(2, "0");
    var day   = fecha.getDate().toString().padStart(2, "0");

    var fechaFin =  new Date(this.fechaFin);
    var year1  = fechaFin.getFullYear();
    var month1 = (fechaFin.getMonth() + 1).toString().padStart(2, "0");
    var day1   = fecha.getDate().toString().padStart(2, "0");

    //console.log('fecha inicial',  new Date(this.fechaPedido).getFullYear());
    //console.log('fecha inicial',  fecha.getFullYear());

    const workbook = new Workbook();
    const worksheet = workbook.addWorksheet('Ventas_del_Dia');

    exportDataGrid({
      component: e.component,
      worksheet,
      topLeftCell: { row: 4, column: 1 },
    }).then((cellRange) => {
      // header
      const headerRow = worksheet.getRow(2);
      headerRow.height = 30;
      worksheet.mergeCells(2, 1, 2, 5);

      headerRow.getCell(1).value = 'DETALLE DE VENTAS DEL DIA';
      headerRow.getCell(1).font = { name: 'Segoe UI Light', size: 22 };
      headerRow.getCell(1).alignment = { horizontal: 'center' };

      const headerRow1 = worksheet.getRow(3);
      headerRow1.height = 30;
      worksheet.mergeCells(3, 1, 3, 5);



      headerRow1.getCell(1).value = 'DESDE ' +fecha.getFullYear() + "/"
        + (fecha.getMonth() + 1)
        + "/" + fecha.getDate()
        + ' HASTA ' +fechaFin.getFullYear() + "/"
        + (fechaFin.getMonth() + 1)
        + "/" + fechaFin.getDate() ;
      headerRow1.getCell(1).font = { name: 'Segoe UI Light', size: 18 };
      headerRow1.getCell(1).alignment = { horizontal: 'center' };


    }).then(() => {
      workbook.xlsx.writeBuffer().then((buffer) => {
        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), year + month + day + '_al_'+ year1 + month1 + day1 +'_Ventas_de_Dia_Menu.xlsx');
      });
    });
    //e.cancel = true;
  }

}
