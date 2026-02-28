import { Component, OnInit } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import { ProductosAlmancen } from 'src/app/interfaces/inventario/ProductosAlmacen';
import { InventarioService } from 'src/app/services/inventario.service';
import DxDataGrid from 'devextreme/ui/data_grid';
import { InventarioAsignacion } from 'src/app/interfaces/inventario/InventarioAsignacion';
import { InventarioProducto } from 'src/app/interfaces/inventario/InventarioProducto';
import { AlmacenDTO } from 'src/app/interfaces/inventario/Almacen';
import { VentaService } from 'src/app/services/venta.service';


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
  bloquearDestino = false;
  constructor(
    private inventarioService: InventarioService,
    private ventaService: VentaService,
  ) {

  }

  ngOnInit() {

    this.barraOrigen = new AlmacenDTO();
    this.barraDestino = new AlmacenDTO();

    this.inventarioService.obtenerAlmacenesParaAsignacion(0).then(resul => resul.subscribe(data => {
      this.listaAlmancenOrigen = data.listEntities;
      //console.log('lista111111', this.listaAlmancenOrigen);
      //this.selectProductosAlmacenOrig.idAlmacen = 1;
      //this.barraOrigen.idAlmacen = 1;
    }));

    this.inventarioService.obtenerAlmacenesParaAsignacion(0).then(resul => resul.subscribe(data => {
      this.listaDestino = data.listEntities.filter(a => a.descripcion?.toLowerCase() !== 'proveedor');
      //this.selectProductosAlmacenDest.idAlmacen = 11;
      //this.barraDestino.idAlmacen = 11;
    }));
    this.barraOrigen.idAlmacen = 11;
    this.barraDestino.idAlmacen = 1;
    this.bloquearDestino = true;
    this.obtieneProductos();
  }

  onRowValidating(e: any) {
  // toma nuevo valor si viene en newData, sino conserva el anterior
  const cantidad = e.newData?.cantidadAsignada ?? e.oldData?.cantidadAsignada ?? 0;
  const disponible = e.oldData?.disponibleAlmacenCentral ?? 0;

  if (cantidad < 0) {
    e.isValid = false;
    e.errorText = "No se permiten valores negativos en 'Asignar'.";
    return;
  }

  if (cantidad > disponible) {
    e.isValid = false;
    e.errorText = `La cantidad asignada (${cantidad}) no puede ser mayor al disponible (${disponible}).`;
    return;
  }
}
  obtieneProductos(){
    this.inventarioService.ObtenerProductosAlmacenCentral(this.barraOrigen.idAlmacen).then(resul => resul.subscribe(data => {
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
    this.inventarioAsignacion.origen = this.listaAlmancenOrigen.find(a => a.idAlmacen === this.barraOrigen.idAlmacen).descripcion;
    this.inventarioAsignacion.destino = this.listaDestino.find(a => a.idAlmacen === this.barraDestino.idAlmacen).descripcion;
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
          inventarioProducto.fechaDeVencimiento = x.key.fechaDeVencimiento;
        else
          inventarioProducto.fechaDeVencimiento = x.data.fechaDeVencimiento;
        console.log('fecha', inventarioProducto.fechaDeVencimiento);

        this.inventarioAsignacion.detalleProductos.push(inventarioProducto);
      }
    });

    (await this.inventarioService.GrabaAsignacionProducto(this.inventarioAsignacion)).subscribe(resul => {
      console.log(resul.message);
      this.inventarioService.showMessageSucess(resul.message);
      //Imprimir voucher
      if (resul.state === 1) {
            this.ventaService.downLoadFile(resul.code, 'application/pdf');
      }
      ///
      this.listaProductosAlmacen = [];
      this.inventarioService.ObtenerProductosAlmacenCentral(this.barraOrigen.idAlmacen).then(resul => resul.subscribe(data => {
        this.listaProductosAlmacen = data.listEntities;
        this.dataSource = new DataSource(this.listaProductosAlmacen);
      }));

      component.refresh(true);
      component.cancelEditData();
    });

  }

  selectBarraOrigen(event: any) {
    //console.log('barra origen', event.detail);
    const idSeleccionado = event.detail.value;
    //console.log('ID origen', idSeleccionado);
    this.barraOrigen = new AlmacenDTO();
    this.barraOrigen.idAlmacen = idSeleccionado ;

    if (idSeleccionado === 11) { // 11 = Proveedor
      this.barraDestino.idAlmacen = 1; // 1 = Almacén Central
      this.bloquearDestino = true;
    } else {
      this.bloquearDestino = false;
    }
    this.obtieneProductos();
    //console.log('bloquearDestino', this.bloquearDestino);

  }

  selectBarraDestino(event) {
    if (this.bloquearDestino) return;
    this.barraDestino = new AlmacenDTO();
    this.barraDestino.idAlmacen = event.detail.value;
  }

  validarCantidadAsignada = (e: any): boolean => {
  const cantidad = e.value;

  // En batch, el disponible está en la fila (data)
  const disponible = e.data?.disponibleAlmacenCentral ?? 0;

  if (cantidad == null) return true; // si quieres obligatorio, usa required en HTML

  if (cantidad < 0) return false;

  if (cantidad > disponible) return false;

  return true;
  };



}
