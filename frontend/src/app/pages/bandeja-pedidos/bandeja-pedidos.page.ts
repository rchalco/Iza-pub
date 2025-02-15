import { Component, OnInit, NgModule } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import { DetallePedidosDTO } from 'src/app/interfaces/venta/detallePedido';
import { VentaService } from 'src/app/services/venta.service';
import notify from 'devextreme/ui/notify';
import { exportDataGrid } from 'devextreme/excel_exporter';
import { Workbook } from 'exceljs';
import { saveAs } from 'file-saver-es';

@Component({
  selector: 'app-bandeja-pedidos',
  templateUrl: './bandeja-pedidos.page.html',
  styleUrls: ['./bandeja-pedidos.page.scss'],
})
export class BandejaPedidosPage implements OnInit {

  fechaPedido: any = new Date();
  fechaPedidoFin: any = new Date();
  listaPedidos: DetallePedidosDTO[] = [];
  dataSourcePedidos: DataSource = new DataSource(this.listaPedidos);
  showDetalle = true;
  popupVisible = false;
  idformaPago = 0;
  idPedidoFormaPago = 0;
  closeButtonOptions: Record<string, unknown>;
  aceptarButtonOptions: Record<string, unknown>;
  aceptaruttonOptions: Record<string, unknown>;
  //fechaPedido: any = new Date();
  min: Date = new Date(2024, 11, 1);
  now: Date = new Date();


  constructor(private ventaService: VentaService) { 

    this.closeButtonOptions = {
      text: 'Close',
      stylingMode: 'outlined',
      type: 'normal',
      onClick: () => {
        this.popupVisible = false;
      },
    };

    this.aceptaruttonOptions = {
      icon: 'send',
      stylingMode: 'contained',
      text: 'Cambiar',
      onClick: () => {
        this.cambiarFormaPago(this.idformaPago);
        
        
        const message = 'Se realizo la forma de pago';
        notify({
          message,
          position: {
            my: 'center top',
            at: 'center top',
          },
        }, 'success', 3000);
        this.popupVisible = false;
      },
    };


  }

  ngOnInit() {
    this.fechaPedido = new Date(this.now);
    this.fechaPedidoFin = new Date(this.now);
    this.buscarPedidos();
    this.anularPedido = this.anularPedido.bind(this);
    this.formaPago = this.formaPago.bind(this);
    //this.popupVisible = this.popupVisible.bind(this);
  }

  buscarPedidos() {

    console.log('Fecha Pedido',this.fechaPedido);

    this.listaPedidos = [];
    this.showDetalle = true;
    this.ventaService.detallePedidoPorFormaPago(this.fechaPedido, this.fechaPedidoFin).then((service) => {
      service.subscribe((resul) => {
        this.listaPedidos = resul.listEntities;
        this.dataSourcePedidos = resul.listEntities;
        this.ventaService.showMessageResponse(resul);

      });
    });
  }

  anularPedido(e: any) {
    console.log('anula pedidoooooooo',e.row.data);
    let pedido = new DetallePedidosDTO();
    pedido = e.row.data;
    this.anular(pedido);
   
  }

  anular(pedido: DetallePedidosDTO) {
    this.ventaService.anulaPedido(pedido.idPedMaster).then((service) => {
      service.subscribe((resul) => {
        this.listaPedidos = resul.listEntities;
        this.dataSourcePedidos = resul.listEntities;
        this.ventaService.showMessageResponse(resul);
      });
    });
  }


  formaPago(e: any) {
    if (e!=null)
    {
      let pedido = new DetallePedidosDTO();
      pedido = e.row.data;
      this.idPedidoFormaPago = pedido.idPedFormaPago;
      this.popupVisible = true;

      console.log('popup',this.popupVisible);
      console.log('popupeeeeeee',this.idPedidoFormaPago);
    }


  }

  setFormaPago(pago: any) {
    console.log("cambio forma pago", pago.detail.value);
    this.idformaPago = parseInt(pago.detail.value);
  }

  cambiarFormaPago(_idFormaPago){
    this.ventaService.actualizaFormaPagoPedido(this.idPedidoFormaPago,_idFormaPago).then((service) => {
      service.subscribe((resul) => {
        this.listaPedidos = resul.listEntities;
        this.dataSourcePedidos = resul.listEntities;
        //this.ventaService.showMessageResponse(resul);
      });
    });
  }

  onExporting(e: any) {
    //this.fechaPedido = Date.now();
    var fecha =  new Date(this.fechaPedido);
    var year  = fecha.getFullYear();
    var month = (fecha.getMonth() + 1).toString().padStart(2, "0");
    var day   = fecha.getDate().toString().padStart(2, "0");

    var fechaFin =  new Date(this.fechaPedidoFin);
    var year1  = fechaFin.getFullYear();
    var month1 = (fechaFin.getMonth() + 1).toString().padStart(2, "0");
    var day1   = fecha.getDate().toString().padStart(2, "0");

    //console.log('fecha inicial',  new Date(this.fechaPedido).getFullYear());
    console.log('fecha inicial',  fecha.getFullYear());
    
    const workbook = new Workbook();
    const worksheet = workbook.addWorksheet('Pedidos_del_Dia');

    exportDataGrid({
      component: e.component,
      worksheet,
      topLeftCell: { row: 4, column: 1 },
    }).then((cellRange) => {
      // header
      const headerRow = worksheet.getRow(2);
      headerRow.height = 30;
      worksheet.mergeCells(2, 1, 2, 5);

      headerRow.getCell(1).value = 'DETALLE DE PEDIDOS DEL DIA';
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
        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), year + month + day + '_'+ year1 + month1 + day1 +'_Pedidos_de_Fecha.xlsx');
      });
    });
    //e.cancel = true;
  }


}
