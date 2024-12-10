import { Component, OnInit } from '@angular/core';
import { InventarioProducto } from 'src/app/interfaces/inventario/InventarioProducto';
import { InventarioService } from 'src/app/services/inventario.service';

@Component({
  selector: 'app-cierre-inventario',
  templateUrl: './cierre-inventario.page.html',
  styleUrls: ['./cierre-inventario.page.scss'],
})
export class CierreInventarioPage implements OnInit {

  listaInventario: InventarioProducto[];
  showAperturaInvetario = false;
  showDetalleInvetario = false;

  constructor(private inventarioService: InventarioService) { }

  ngOnInit() {
    this.showAperturaInvetario = true;
    this.showDetalleInvetario = false;
  }

  cierreInventario() {
  console.log('CIERRE INVENTARIO');  
    this.inventarioService
      .cierreInventario()
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
   
  }

}
