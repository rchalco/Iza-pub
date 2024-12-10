import { Component, OnInit } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import { IngredientesDeMenuGeneralDTO } from 'src/app/interfaces/inventario/IngredientesDeMenuGeneral';
import { DetalleVenta } from 'src/app/interfaces/venta/detalleVenta';
import { InventarioService } from 'src/app/services/inventario.service';
import { VentaService } from 'src/app/services/venta.service';

@Component({
  selector: 'app-venta-busqueda',
  templateUrl: './venta-busqueda.page.html',
  styleUrls: ['./venta-busqueda.page.scss'],
})
export class VentaBusquedaPage implements OnInit {

  showMain = true;
  totalVenta = 0;
  showMessageErrorNOCajaAbierta = false;
  idAlmacen = 0;
  productos: IngredientesDeMenuGeneralDTO[] = [];
  productosVender: IngredientesDeMenuGeneralDTO[] = [];
  listaDetalleVentas: DetalleVenta[] = [];
  listFormasDePago = [];
  //dataSourceProductos: DataSource = new DataSource(this.productos);
  //dataSourceProductosVender: DataSource = new DataSource(this.productosVender);
  textoBuscar: string = '';


  constructor(
    private inventarioService: InventarioService,
    private ventaService: VentaService,
    ) {
    this.adicionarProducto = this.adicionarProducto.bind(this);
    this.eliminarProducto = this.eliminarProducto.bind(this);
   }

  ngOnInit() {
    this.showMain = true;
    this.productosVender = [];
    this.inventarioService.getInfoEviroment().then((x) => {
      if (x.idAlmacen !== 0 && x.idOperacionDiariaCaja !== 0) {
        //this.BuscarProductos(x.idAlmacen);
        this.idAlmacen = x.idAlmacen;
      } else {
        this.showMessageErrorNOCajaAbierta = true;
      }
    });


  }

  buscarProductos() {
    this.inventarioService
      .busquedaMenuGeneral(this.textoBuscar)
      .then((productosService) => {
        productosService.subscribe((resul) => {
          console.log(resul);
          this.productos = resul.listEntities;
          //this.dataSourceProductos = new DataSource(this.productos);
          console.log('productos', resul);
          this.productos.forEach((x) => {
            x.embase = x.embase ? x.embase.toUpperCase() : 'UNIDAD';
          });
        });
      });
  }
  recalculaTotalPagar(){
    console.log('ELIMINAR INGRE');
    if(this.productosVender== null || this.productosVender.length ==0)
      return;
    this.totalVenta = 0;
    for (var i = 0; i < this.productosVender.length; i++) {
      this.totalVenta += (this.productosVender[i].cantidad * this.productosVender[i].precioUnitario);
    }
  }

  adicionarProducto(e: any) {
    //console.log(e.row.data);
    let ingrediente = new IngredientesDeMenuGeneralDTO();
    ingrediente = e.row.data;

    let lista: IngredientesDeMenuGeneralDTO[] = [];
    lista = this.productosVender.filter(x=>x.idPrecio == ingrediente.idPrecio);
    //console.log('Producto a vender',this.productosVender);
    //console.log('adicionar Prod', lista);
    if (lista.length > 0)
      return;

    ingrediente.subTotal = ingrediente.cantidad * ingrediente.precioUnitario
    this.productosVender.push(ingrediente);
    //this.mostrarIngredientes(ingrediente);
    
  }

  subTotalCellValue(rowData){  
    //console.log('cantidad', rowData);
    //this.recalculaTotalPagar();
    //console.log('subtotal', rowData);
    //console.log('prd vender', this.productosVender.length);
    
    //let subT = Math.round((rowData.cantidad * rowData.precioUnitario));
    let subT = (rowData.cantidad * rowData.precioUnitario);
   
 
   //this.totalVenta = 500;
   
    return subT;  
  }  

  onRecalculaMonto(rowData){  
    this.recalculaTotalPagar();
   console.log('valorrrrrr', rowData);
   
  }  

  logEvent(eventName) {
    console.log('valorrrrrr', eventName);
  }


  eliminarProducto(e: any) {
    
    let ingrediente = new IngredientesDeMenuGeneralDTO();
    ingrediente = e.row.data;
    //this.listaDetalleProductoAGrabar

    for (var i = 0; i < this.productosVender.length; i++) {

      if (this.productosVender[i] === ingrediente) {
        ingrediente.cantidad = 0;
        this.productosVender.splice(i, 1);
      }
    }
    ///
    
    this.recalculaTotalPagar();
  }

  

  realizarPago(_idFormaPago) {
    console.log('PAGAR',this.productosVender);

    if (this.productosVender.length === 0) {
      this.inventarioService.showMessageWarning(
        'Debe seleccionar productos antes de realizar un pago!'
      );
      return;
    }

    let lista: IngredientesDeMenuGeneralDTO[] = [];
    lista = this.productosVender.filter(x=>x.cantidad <= 0);
    if (lista.length > 0){
      this.inventarioService.showMessageWarning(
        'Los productos a vender deben tener la cantidad Mayor a Cero'
      );
      return;
    }
    ///formad de pago mix
    if (_idFormaPago === -1) {
      this.showMain = false;
      //this.showCobrar = true;
      //this.showPOS = false;
      return;
    }
    ///formad de pago tarjeta
    // if (_idFormaPago === 5) {
    //   this.showMain = false;
    //   this.showCobrar = false;
    //   this.showPOS = true;
    //   this.formaPagoActual = _idFormaPago;
    //   setTimeout(() => {
    //     this.cardInput.initReadCard();
    //   }, 1000);

    //   return;
    // }

    this.listaDetalleVentas = [];
    this.listFormasDePago = [];

    const formaDePago = {
      idPedidoMaestro: 0,
      idFormaPago: _idFormaPago,
      montoCubierto: this.totalVenta,
      Diferencia: 0,
    };
    this.listFormasDePago.push(formaDePago);

    this.productosVender.forEach((x) => {
      const detalleventaInstance = new DetalleVenta();
      detalleventaInstance.idProducto = x.idProducto;
      detalleventaInstance.idParamPrecio = x.idPrecio;
      detalleventaInstance.cantidad = x.cantidad;
      detalleventaInstance.precioUnitario = x.precio;
      detalleventaInstance.unidadePorCaja = 1;
      detalleventaInstance.precioCaja = 1;
      detalleventaInstance.nombreProducto = x.nombreProducto;
      this.listaDetalleVentas.push(detalleventaInstance);
    });
    console.log('idAlmacen', this.idAlmacen);

    this.ventaService
    .registrarVenta(
      this.listaDetalleVentas,
      this.listFormasDePago,
      this.idAlmacen,''
    )
    .then((registroService) => {
      registroService.subscribe((resul) => {
        this.ventaService.showMessageResponse(resul);
        
          this.productosVender = [];
          this.listaDetalleVentas = [];
          this.totalVenta = 0;
          this.listFormasDePago = [];
          this.productos=  [];
        
      });
    });

  }

  validateNumber(e) {
    return e.value > 0;
}

}
