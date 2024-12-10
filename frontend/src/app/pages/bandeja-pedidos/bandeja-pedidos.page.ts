import { Component, OnInit, NgModule } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import { DetallePedidosDTO } from 'src/app/interfaces/venta/detallePedido';
import { VentaService } from 'src/app/services/venta.service';
import notify from 'devextreme/ui/notify';

@Component({
  selector: 'app-bandeja-pedidos',
  templateUrl: './bandeja-pedidos.page.html',
  styleUrls: ['./bandeja-pedidos.page.scss'],
})
export class BandejaPedidosPage implements OnInit {

  fechaPedido: any = new Date();
  min: Date = new Date(2024, 10, 1);
  now: Date = new Date();
  listaPedidos: DetallePedidosDTO[] = [];
  dataSourcePedidos: DataSource = new DataSource(this.listaPedidos);
  showDetalle = true;
  popupVisible = false;
  idformaPago = 0;
  idPedidoFormaPago = 0;
  closeButtonOptions: Record<string, unknown>;
  aceptarButtonOptions: Record<string, unknown>;
  aceptaruttonOptions: Record<string, unknown>;

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
    this.buscarPedidos();
    this.anularPedido = this.anularPedido.bind(this);
    this.formaPago = this.formaPago.bind(this);
    //this.popupVisible = this.popupVisible.bind(this);
  }

  buscarPedidos() {
    this.listaPedidos = [];
    this.showDetalle = true;
    this.ventaService.detallePedidoPorFormaPago().then((service) => {
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


}
