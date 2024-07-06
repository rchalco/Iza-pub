import { Component, OnInit } from '@angular/core';
import { TouchSequence } from 'selenium-webdriver';
import { MDPedidosPorEntregar } from 'src/app/interfaces/tintoreria/ResponsePedidosEntregar';
import { DetalleVenta } from 'src/app/interfaces/venta/detalleVenta';
import { ResulProductoPrecioVenta } from 'src/app/interfaces/venta/itemProductoVenta';
import { PersonaService } from 'src/app/services/persona.service';
import { StockService } from 'src/app/services/stock.service';
import { TintoreriaService } from 'src/app/services/tintoreria.service';
import { VentaService } from 'src/app/services/venta.service';

@Component({
  selector: 'app-tintoreria-entrega',
  templateUrl: './tintoreria-entrega.page.html',
  styleUrls: ['./tintoreria-entrega.page.scss'],
})
export class TintoreriaEntregaPage implements OnInit {
  listaPedidos: MDPedidosPorEntregar[];
  textoBusacar: string = '';
  constructor(
    private tintoreriaService: TintoreriaService,
    private personaService: PersonaService
  ) {}

  ngOnInit() {
    this.textoBusacar = '';
    this.cargarPedidosPendientes();
  }

  cargarPedidosPendientes() {
    this.tintoreriaService
      .obtienePedidosPorEntregar()
      .then((productosService) => {
        productosService.subscribe((resul) => {
          console.log(resul);
          this.listaPedidos = resul.listEntities as MDPedidosPorEntregar[];
          console.log('pedidos', this.listaPedidos);
        });
      });
  }

  buscarPedidos(event) {
    this.textoBusacar = event.detail.value;
  }

  entregarPedido(idPedidoMaster) {
    this.tintoreriaService
      .entregarPedido(idPedidoMaster)
      .then((productosService) => {
        productosService.subscribe((resul) => {
          console.log('resul entregarPedido', resul);
          this.tintoreriaService.showMessageResponse(resul);
          this.cargarPedidosPendientes();
        });
      });
  }
}
