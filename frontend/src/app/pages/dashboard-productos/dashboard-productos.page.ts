import { Component, OnInit } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import { DashboardProductosDTO } from 'src/app/interfaces/inventario/DashboardProductos';
import { ProductosVendidosPorBarraDTO } from 'src/app/interfaces/inventario/ProductosVendidosPorBarra';
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

  listaVendidos: ProductosVendidosPorBarraDTO[] = [];
  dataSourceVendidos: DataSource = new DataSource(this.listaVendidos);

  listaAlmancenOrigen: [];
  tipoReporte = 0;
  barraOrigen;
  constructor(private inventarioService: InventarioService) { }

  ngOnInit() {
    this.inventarioService.obtenerDashboardProductos().then((service) => {
      service.subscribe((response) => {
        this.listaProductos = response.listEntities;
        this.dataSourceProductos = response.listEntities;
      });
    });
    this.inventarioService.obtenerAlmacenesParaAsignacion().then(resul => resul.subscribe(data => {
      this.listaAlmancenOrigen = data.listEntities;
    }));

  }

  actualizarDatos(event) {
    this.inventarioService.obtenerDashboardProductos().then((service) => {
      service.subscribe((response) => {
        this.listaProductos = response.listEntities;
        this.dataSourceProductos = response.listEntities;
      });
    });
  }

  setReporte(reporte: any) {
    console.log("reporte", reporte.detail.value);
    this.tipoReporte = parseInt(reporte.detail.value);
  }

  selectBarraOrigen(event) {
    this.barraOrigen = event.detail.value.idAlmacen;
    console.log("ALmacen", this.barraOrigen);
    
  }

  generarResumenProductos(){

    console.log("ALmacen", this.barraOrigen);
    this.inventarioService.productosVendidosPorBarra(this.barraOrigen).then((service) => {
      service.subscribe((response) => {
        this.listaVendidos = response.listEntities;
        this.dataSourceVendidos = response.listEntities;
      });
    });
  }
}
