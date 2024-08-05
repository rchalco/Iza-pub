import { Component, OnInit } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import { DashboardProductosDTO } from 'src/app/interfaces/inventario/DashboardProductos';
import { InventarioService } from 'src/app/services/inventario.service';
import { StockService } from 'src/app/services/stock.service';

@Component({
  selector: 'app-dashboard-productos',
  templateUrl: './dashboard-productos.page.html',
  styleUrls: ['./dashboard-productos.page.scss'],
})
export class DashboardProductosPage implements OnInit {

  listaProductos: DashboardProductosDTO[] = [];
  dataSourceProductos: DataSource = new DataSource(this.listaProductos);

  constructor(private inventarioService: InventarioService) { }

  ngOnInit() {
    this.inventarioService.obtenerDashboardProductos().then((service) => {
      service.subscribe((response) => {
        this.listaProductos = response.listEntities;
        this.dataSourceProductos = response.listEntities;
      });
    });

  }

  actualizarDatos(event) {
    this.inventarioService.obtenerDashboardProductos().then((service) => {
      service.subscribe((response) => {
        this.listaProductos = response.listEntities;
        this.dataSourceProductos = response.listEntities;
      });
    });
  }

}
