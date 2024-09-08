import { Component, OnInit } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import { ProductosAlmancen } from 'src/app/interfaces/inventario/ProductosAlmacen';
import { InventarioService } from 'src/app/services/inventario.service';
import DxDataGrid from 'devextreme/ui/data_grid';
import { InventarioAsignacion } from 'src/app/interfaces/inventario/InventarioAsignacion';
import { InventarioProducto } from 'src/app/interfaces/inventario/inventarioProducto';


@Component({
  selector: 'app-asignacion-productos',
  templateUrl: './asignacion-productos.page.html',
  styleUrls: ['./asignacion-productos.page.scss'],
})
export class AsignacionProductosPage implements OnInit {

  listaProductosAlmacen: ProductosAlmancen[] = [];
  dataSource: DataSource = new DataSource(this.listaProductosAlmacen);
  listaAlmancenOrigen: [];
  listaDestino: [];
  inventarioAsignacion: InventarioAsignacion = new InventarioAsignacion();
  barraOrigen;
  barraDestino;

  constructor(private inventarioService: InventarioService) {

  }

  ngOnInit() {

    this.inventarioService.ObtenerProductosAlmacenCentral().then(resul => resul.subscribe(data => {
      this.listaProductosAlmacen = data.listEntities;
      this.dataSource = new DataSource(this.listaProductosAlmacen);
    }));

    this.inventarioService.obtenerAlmacenesParaAsignacion().then(resul => resul.subscribe(data => {
      this.listaAlmancenOrigen = data.listEntities;
    }));

    this.inventarioService.obtenerAlmacenesParaAsignacion().then(resul => resul.subscribe(data => {
      this.listaDestino = data.listEntities;
    }));

  }

  onSaving(e: any) {
    e.cancel = true;

    if (e.changes.length) {
      e.promise = this.processBatchRequest(`${URL}/Batch`, JSON.stringify(e.changes), e.component);
    }
  }

  async processBatchRequest(url: string, changes: string, component: DxDataGrid): Promise<any> {

    this.inventarioAsignacion.idAlmacenDesde = this.barraOrigen.idAlmacen;
    this.inventarioAsignacion.idAlmacenHasta = this.barraDestino.idAlmacen;
    this.inventarioAsignacion.observaciones = '';
    this.inventarioAsignacion.origen = this.barraOrigen.descripcion;
    this.inventarioAsignacion.destino = this.barraDestino.descripcion;
    this.inventarioAsignacion.detalleProductos = [];
    const listaCamabiada = JSON.parse(changes);
    console.log('listaCamabiada', listaCamabiada);
    listaCamabiada.forEach(x => {
      if (x.data.cantidadAsignada > 0) {
        const inventarioProducto = new InventarioProducto();
        inventarioProducto.cantidad = x.data.cantidadAsignada * 100 / 100.00;
        inventarioProducto.cantidadCaja = 0.00;
        inventarioProducto.idProducto = x.key.idProducto;
        inventarioProducto.idProveedor = 0;
        inventarioProducto.idTipo = 0;
        inventarioProducto.montoCompra = 0.00;
        inventarioProducto.montoTotal = 0.00;
        inventarioProducto.nombreProducto = x.key.nombreProducto;
        inventarioProducto.categoria = "";
        inventarioProducto.enStock = 0.00;
        this.inventarioAsignacion.detalleProductos.push(inventarioProducto);
      }
    });

    (await this.inventarioService.GrabaAsignacionProducto(this.inventarioAsignacion)).subscribe(resul => {
      console.log(resul.message);
      this.inventarioService.showMessageSucess(resul.message);

      this.inventarioService.ObtenerProductosAlmacenCentral().then(resul => resul.subscribe(data => {
        this.listaProductosAlmacen = data.listEntities;
        this.dataSource = new DataSource(this.listaProductosAlmacen);
      }));

      component.refresh(true);
      component.cancelEditData();
    });

  }

  selectBarraOrigen(event) {
    this.barraOrigen = event.detail.value;
  }

  selectBarraDestino(event) {
    this.barraDestino = event.detail.value;
  }
}