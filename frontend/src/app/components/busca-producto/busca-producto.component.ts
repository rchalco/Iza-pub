import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ResulProductoPrecioVenta } from 'src/app/interfaces/venta/itemProductoVenta';
import { VentaService } from 'src/app/services/venta.service';

@Component({
  selector: 'app-busca-producto',
  templateUrl: './busca-producto.component.html',
  styleUrls: ['./busca-producto.component.scss'],
})
export class BuscaProductoComponent implements OnInit {
  productos: ResulProductoPrecioVenta[] = [];
  textoBusacar = '';

  @Input() public nameButtonSearch: string;
  @Output() public selectProducto: EventEmitter<ResulProductoPrecioVenta> =
    new EventEmitter<ResulProductoPrecioVenta>();

  constructor(private ventaService: VentaService) {}

  ngOnInit() {
    this.cargarProductos();
  }

  cargarProductos() {
    this.ventaService.searchProductVenta().then((productosService) => {
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

  buscar(event) {
    this.textoBusacar = event.detail.value;
  }

  clickEventSearchItem(producto) {
    console.log("clickEventSearchItem", producto, this.selectProducto);
    if (this.selectProducto) {
      this.selectProducto.emit(producto);
    }
  }
}
