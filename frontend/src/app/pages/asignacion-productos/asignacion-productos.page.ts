import { Component, OnInit } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import { ProductosAlmancen } from 'src/app/interfaces/inventario/ProductosAlmacen';
import { InventarioService } from 'src/app/services/inventario.service';

@Component({
  selector: 'app-asignacion-productos',
  templateUrl: './asignacion-productos.page.html',
  styleUrls: ['./asignacion-productos.page.scss'],
})
export class AsignacionProductosPage implements OnInit {

  listaProductosAlmacen: ProductosAlmancen[] = [];
  dataSource: DataSource = new DataSource(this.listaProductosAlmacen);

  constructor(private inventarioService: InventarioService) {

  }

  ngOnInit() {
    console.log("********init");

    this.inventarioService.ObtenerProductosAlmacenCentral().then(resul => resul.subscribe(data => {
      this.listaProductosAlmacen = data.listEntities;
      console.log("Info alamacen central", this.listaProductosAlmacen);
      this.dataSource = new DataSource(this.listaProductosAlmacen);
    }));
    //this.dataSource.reload();
  }

}
