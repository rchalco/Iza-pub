import { Component, OnInit, ViewChild } from '@angular/core';
import { ListaProductoComponent } from 'src/app/components/lista-producto/lista-producto.component';
import { InventarioProducto } from 'src/app/interfaces/inventario/InventarioProducto';
import { InventarioService } from 'src/app/services/inventario.service';
import { StockService } from 'src/app/services/stock.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-apertura-inventario',
  templateUrl: './apertura-inventario.page.html',
  styleUrls: ['./apertura-inventario.page.scss'],
})
export class AperturaInventarioPage implements OnInit {
  showAperturaInvetario = false;
  showDetalleInvetario = false;
  listaInventario: InventarioProducto[];
  @ViewChild('listaProductoComponent') listaProductoComponent: ListaProductoComponent;

  constructor(private inventarioService: InventarioService) { }

  ngOnInit() {
    this.showAperturaInvetario = true;
    this.showDetalleInvetario = false;
  }

  abrirInventario() {

    this.inventarioService
      .aperturaInventario()
      .then((productosService) => {
        productosService.subscribe((resul) => {
          this.inventarioService.showMessageResponse(resul);
          this.listaInventario = resul.listEntities as InventarioProducto[];
          console.log('pedidos', this.listaInventario);
          /*
          this.listaPedidos.forEach((zz) => {
            this.total += zz.total;
            this.montoApertura = zz.montoApertura;
            this.montoCierre = zz.montoCierre;
          });
          console.log('pedidos', this.listaPedidos);
          */
        });
      });
    //if (this.listaInventario.length<=0)
    //  return;

    this.showAperturaInvetario = false;
    this.showDetalleInvetario = true;
  }
  asignacion() { 
    console.log("lista de productos internos", this.listaProductoComponent.productos);
  }

  
}
