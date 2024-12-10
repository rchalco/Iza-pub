import { Component, OnInit } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import { ProductosAlmancen } from 'src/app/interfaces/inventario/ProductosAlmacen';
import { InventarioService } from 'src/app/services/inventario.service';
import DxDataGrid from 'devextreme/ui/data_grid';
import { InventarioAsignacion } from 'src/app/interfaces/inventario/InventarioAsignacion';
import { InventarioProducto } from 'src/app/interfaces/inventario/InventarioProducto';
import { AlmacenDTO } from 'src/app/interfaces/inventario/Almacen';


@Component({
  selector: 'app-asignacion-productos',
  templateUrl: './asignacion-productos.page.html',
  styleUrls: ['./asignacion-productos.page.scss'],
})
export class AsignacionProductosPage implements OnInit {

  listaProductosAlmacen: ProductosAlmancen[] = [];
  dataSource: DataSource = new DataSource(this.listaProductosAlmacen);
  listaAlmancenOrigen: AlmacenDTO[]=[];
  listaDestino: AlmacenDTO[]=[];
  inventarioAsignacion: InventarioAsignacion = new InventarioAsignacion();
  barraOrigen = new AlmacenDTO();
  barraDestino= new AlmacenDTO();

  constructor(private inventarioService: InventarioService) {

  }

  ngOnInit() {

    this.barraOrigen = new AlmacenDTO();
    this.barraDestino = new AlmacenDTO();
    
    this.inventarioService.obtenerAlmacenesParaAsignacion().then(resul => resul.subscribe(data => {
      this.listaAlmancenOrigen = data.listEntities;
      //console.log('lista111111', this.listaAlmancenOrigen);
      //this.selectProductosAlmacenOrig.idAlmacen = 1;
      //this.barraOrigen.idAlmacen = 1;
    }));

    this.inventarioService.obtenerAlmacenesParaAsignacion().then(resul => resul.subscribe(data => {
      this.listaDestino = data.listEntities;
      //this.selectProductosAlmacenDest.idAlmacen = 11;
      //this.barraDestino.idAlmacen = 11;
    }));
    this.obtieneProductos();
    this.barraOrigen.idAlmacen = 11;
    this.barraDestino.idAlmacen = 1;
  }

  obtieneProductos(){
    this.inventarioService.ObtenerProductosAlmacenCentral().then(resul => resul.subscribe(data => {
      this.listaProductosAlmacen = data.listEntities;
      this.dataSource = new DataSource(this.listaProductosAlmacen);
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
    console.log('listaCamabiadaxxxx', this.listaProductosAlmacen);
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
        if (x.data.fechaDeVencimiento === null)
          inventarioProducto.fechaDeVencimiento = x.data.fechaDeVencimiento;
        else
          inventarioProducto.fechaDeVencimiento = x.key.fechaDeVencimiento;
        console.log('fecha', inventarioProducto.fechaDeVencimiento);
        
        this.inventarioAsignacion.detalleProductos.push(inventarioProducto);
      }
    });

    (await this.inventarioService.GrabaAsignacionProducto(this.inventarioAsignacion)).subscribe(resul => {
      console.log(resul.message);
      this.inventarioService.showMessageSucess(resul.message);
      this.listaProductosAlmacen = [];
      this.inventarioService.ObtenerProductosAlmacenCentral().then(resul => resul.subscribe(data => {
        this.listaProductosAlmacen = data.listEntities;
        this.dataSource = new DataSource(this.listaProductosAlmacen);
      }));

      component.refresh(true);
      component.cancelEditData();
    });

  }

  selectBarraOrigen(event: any) {
    console.log('listaCamabiadaaaaaaa', event.detail);
    this.barraOrigen = new AlmacenDTO();
    this.barraOrigen.idAlmacen = event.detail.value;
  }

  selectBarraDestino(event) {
    this.barraDestino = new AlmacenDTO();
    this.barraDestino.idAlmacen = event.detail.value;
  }
}