/* eslint-disable @typescript-eslint/member-ordering */
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { VentaService } from 'src/app/services/venta.service';

@Component({
  selector: 'app-lista-producto',
  templateUrl: './lista-producto.component.html',
  styleUrls: ['./lista-producto.component.scss'],
})
export class ListaProductoComponent implements OnInit {
  public productos: any[] = [];
  @Output() public actioButton: EventEmitter<any> = new EventEmitter<any>();

  constructor(private ventaService: VentaService) {
    //console.log("inciando ListaProductoComponent");
    //this.cargarProductos();
  }
  ngOnInit() {}
  buttonAction(productodet) {
    console.log('int compponente', productodet);
    if (this.actioButton) {
      this.actioButton.emit(productodet);
    }
  }
  cargarProductos(almacen) {
    this.ventaService
      .obtieneProductoInventario(almacen)
      .then((productosService) => {
        productosService.subscribe((resul) => {
          console.log(resul);
          this.productos = resul.listEntities;
          console.log('productos', this.productos);
          this.productos.forEach((x) => {
            x.embase = x.embase ? x.embase.toUpperCase() : 'UNIDAD';
            x.picProducto = 'data:image/jpeg;base64,' + x.picProducto;
          });
        });
      });
  }

  verificarStock(_productodet) {
    console.log('verificarStock', _productodet);
    if (_productodet.cantidad > _productodet.enStock) {
      console.log('cantida no valida');
      _productodet.cantidad = 0;
    }
  }
  resetMontos() {
    this.productos.forEach((x) => {
      x.detalle.forEach((yy) => {
        yy.cantidad = 0;
      });
    });
  }
}
