import { TransaccionVentasDetalleDTO } from './../../interfaces/venta/transaccionVentasDetalle';
import { Component, OnInit } from '@angular/core';
import { LugarConsumoDTO } from 'src/app/interfaces/caja/LugarConsumo';
import { PersonaResumenDTO } from 'src/app/interfaces/general/PersonaResumen';
import { ResulProductoPrecioVenta } from 'src/app/interfaces/venta/itemProductoVenta';
import { StockService } from 'src/app/services/stock.service';
import { environment } from 'src/environments/environment';
import { TransaccionVentasDTO } from 'src/app/interfaces/venta/transaccionVentas';
import { FormaPagoDTO } from 'src/app/interfaces/general/FormaPago';
import { DatosFacturaDTO } from 'src/app/interfaces/general/DatosFactura';
import { DocumentoService } from 'src/app/services/documento.service';
import { DataDocumento } from 'src/app/interfaces/general/documento';

@Component({
  selector: 'app-pedido-mesa',
  templateUrl: './pedido-mesa.page.html',
  styleUrls: ['./pedido-mesa.page.scss'],
})
export class PedidoMesaPage implements OnInit {
  lugarConsumo: LugarConsumoDTO[] = [];
  lugarConsumoLibre: LugarConsumoDTO[] = [];
  listaProductos: ResulProductoPrecioVenta[] = [];
  formaPagoIni:FormaPagoDTO;
  productoControl: ResulProductoPrecioVenta;


  showCardPedido = false;
  showCambioMesa = false;
  showMesasPedido = true;
  showFormaPago = false;
  showDatosFactura = false;


  selectedLugarConsumo: LugarConsumoDTO;
  listaMeseros: PersonaResumenDTO[] = [];
  empleadoSeleccionado: PersonaResumenDTO;
  lugarCambioMesa: LugarConsumoDTO;
  colorEstado: string[] = [];

  listaPedidos: TransaccionVentasDetalleDTO[] = [];
  selectPedido: TransaccionVentasDetalleDTO;

  transaccionVenta: TransaccionVentasDTO;

  datosFactura: DatosFacturaDTO;
  formaPago: FormaPagoDTO;
  totalConsumoFinal:number;
  totalDescuentoFinal:number;

  constructor(private stockService: StockService, private documentoService: DocumentoService) { }

  ngOnInit() {
    this.colorEstado[0] = 'danger';
    this.showCardPedido = false;
    this.obtieneMesas();
    this.stockService.listaMeseros().then((resulPromise) => {
      resulPromise.subscribe((resul) => {
        this.listaMeseros = resul.listEntities;
      });
    });

    //console.log("FIN MESEROS");
  }

  obtieneMesas() {
    this.stockService.lugarConsumo().then((resulPromise) => {
      resulPromise.subscribe((resul) => {
        //console.log(resul);
        this.stockService.showMessageResponse(resul);
        this.lugarConsumo = resul.listEntities;
        this.lugarConsumo.forEach(x => {
          x.colorEstado = x.estadoLugar === 'OCUPADO' ? 'danger' : 'success';
        });
      });
    });
  }


  adicionarAMesa(producto) {
    /* descomentar cuando haya meseros
    if(!this.empleadoSeleccionado){
      this.stockService.showMessageWarning('Debe seleccionar un mesero');
      return;
    }
    */
    this.selectPedido = new TransaccionVentasDetalleDTO();
    this.productoControl = producto;
    //console.log("producto", this.productoControl.idProducto);
    this.selectPedido.idProducto = this.productoControl.idProducto;
    this.selectPedido.cantidadDisponible = producto.enStock;
    this.selectPedido.nombreProducto = producto.producto;
    this.selectPedido.nroPedido = 0;
    this.selectPedido.precioVenta = producto.precio;
    this.selectPedido.cantidad = 0;
    this.selectPedido.total = 0;
    //this.selectPedido.mesero = this.empleadoSeleccionado.nombreCompleto;
    //console.log("TRANSACCION", this.selectPedido);
    //console.log("productoaaaaaa", this.productoControl.idProducto);

    this.listaPedidos.push(this.selectPedido);
    //this.listaProductos.slice(producto)

  }

  nuevoPedido() {

    ///VALIDACIONES
    if (environment.idCaja <= 0) {
      this.stockService.showMessageError('La caja no esta abierta');
      return;
    }

    if (!this.selectedLugarConsumo) {
      this.stockService.showMessageWarning('Debe seleccionar lugar en la sala');
      return;
    }

    if (this.selectedLugarConsumo.estadoLugar == 'OCUPADO') {
      this.stockService.showMessageWarning('El lugar seleccionado esta Ocupado');
      return;
    }
    this.listaPedidos = [];
    /////
    this.showCardPedido = true;
    this.showCambioMesa = false;
    this.showMesasPedido = false;
    this.showDatosFactura = false;

  }

  cancelarPedido() {
    this.showCardPedido = false;
    this.showCambioMesa = false;
    this.showMesasPedido = true;
    this.showDatosFactura = false;

  }

  selectMesero(event) {
    //let empleado: number;
    //empleado = parseInt(event.detail.value);

    //console.log('Selecccionar MESEROXXX', event.detail.value);

    this.empleadoSeleccionado = new PersonaResumenDTO();

    this.empleadoSeleccionado = this.listaMeseros.find( ({ idEmpleado }) => idEmpleado === parseInt(event.detail.value) );
/*
    for (var i = 0; i < this.listaMeseros.length; i++) {
      //console.log(this.listaMeseros[i].idEmpleado);
      if (this.listaMeseros[i].idEmpleado ===  parseInt(event.detail.value)) {
        //console.log('ENCONTRO MESERO');
        this.empleadoSeleccionado = this.listaMeseros[i];
      }
    }
    */
    //this.empleadoSeleccionado = event.detail.value;
    //console.log('Selecccionar MESERO', this.empleadoSeleccionado.nombreCompleto);
  }

  borrarItem(event) {

    for (var i = 0; i < this.listaPedidos.length; i++) {

      if (this.listaPedidos[i] === event) {

        this.listaPedidos.splice(i, 1);
      }

    }

    //this.listaProductos.slice(event,1);
    //console.log('BORRAR ITEM', event);
  }

  seleccionarLugar(lugar) {
    this.selectedLugarConsumo = lugar;
  }

  adicionarConsumo() {
    ///VALIDACIONES
    if (environment.idCaja <= 0) {
      this.stockService.showMessageError('La caja no esta abierta');
      return;
    }

    if (!this.selectedLugarConsumo) {
      this.stockService.showMessageWarning('Debe seleccionar un lugar en la sala');
      return;
    }

    if (this.selectedLugarConsumo.estadoLugar != 'OCUPADO') {
      this.stockService.showMessageWarning('El lugar seleccionado esta Libre, debe seleccionar NUEVO PEDIDO');
      return;
    }
    if (this.selectedLugarConsumo.idTransaccion == null) {
      this.stockService.showMessageWarning('No se recupero ninguna transaccion debe cambiar la mensa a LIBRE');
      return;
    }

    this.listaPedidos = [];
    ///Recuperamos los pedidos anteriores
    this.stockService.transaccionesDetallePorID(this.selectedLugarConsumo.idTransaccion).then((resulPromise) => {
      resulPromise.subscribe((resul) => {
        //console.log(resul);
        this.stockService.showMessageResponse(resul);
        this.listaPedidos = resul.listEntities;
      });
    });
    /////
    this.showCardPedido = true;
    this.showCambioMesa = false;
    this.showMesasPedido = false;
    this.showDatosFactura = false;

  }
  grabarPedido() {
    ///Validaciones
    if (this.listaPedidos == null) {
      this.stockService.showMessageWarning('Debe ingresar algun item al pedido.');
      return;
    }

    if (this.empleadoSeleccionado == null) {
      this.stockService.showMessageWarning('Debe seleccionar un Mesero para el pedido actual.');
      return;
    }

    this.listaPedidos.forEach(x => {
      if (x.cantidad <= 0) {
        this.stockService.showMessageWarning('Todos los pedidos deben tener una cantidad mayor a cero.');
        return;
      }

    });
    ///Mensaje de confirmacion para grabar.
    ///
    if (!this.transaccionVenta){
      this.transaccionVenta = new TransaccionVentasDTO();
      //this.transaccionVenta.transaccionDetalle = new TransaccionVentasDetalleDTO[];
    }


    this.transaccionVenta.idAmbiente = this.selectedLugarConsumo.idLugarFisico;
    this.transaccionVenta.idCajaOperacionDiariaCaja = environment.idCaja;
    this.transaccionVenta.idcTipoTransaccion = 85;
    this.transaccionVenta.fechaHora = this.selectedLugarConsumo.fechaHora;

    //console.log('IDTRANSACCION',this.selectedLugarConsumo.idTransaccion);

    this.transaccionVenta.idPedMaster = 0;
    if (this.selectedLugarConsumo.idTransaccion)
      this.transaccionVenta.idPedMaster = this.selectedLugarConsumo.idTransaccion;

    this.transaccionVenta.idcEstado = 27;

    //console.log('PEDIDOS',this.listaPedidos);
    this.transaccionVenta.transaccionDetalle = this.listaPedidos;


    this.stockService.grabaPedido(this.transaccionVenta).then((resulPromise) => {
      resulPromise.subscribe((resul) => {
        //console.log(resul);
        this.stockService.showMessageResponse(resul);
        if (resul.state === 1) {
          this.imprimirComanda();
        }
      });
    });


    this.obtieneMesas();
    this.transaccionVenta = null;
    this.listaPedidos = null;
    this.empleadoSeleccionado = null;
    this.showCardPedido = false;
    this.showCambioMesa = false;
    this.showMesasPedido = true;
    this.showDatosFactura = false;
  }

  cambioMesa() {

    if (!this.selectedLugarConsumo) {
      this.stockService.showMessageWarning('Debe seleccionar un lugar en la sala');
      return;
    }

    if (this.selectedLugarConsumo.estadoLugar != 'OCUPADO') {
      this.stockService.showMessageWarning('El lugar seleccionado se encuentra libre.');
      return;
    }

    this.showCardPedido = false;
    this.showCambioMesa = true;
    this.showMesasPedido = false;
    this.showDatosFactura = false;

    this.lugarConsumoLibre = this.lugarConsumo.filter(x => x.estadoLugar != 'OCUPADO');
    //console.log('Mesas libres', this.lugarConsumoLibre)

  }


  cancelarCambioMesa() {

    this.showCardPedido = false;
    this.showCambioMesa = false;
    this.showMesasPedido = true;
    this.showDatosFactura = false;
    this.lugarConsumoLibre = null;
    this.selectedLugarConsumo = null;
  }

  selectMesaNueva(event) {
    this.lugarCambioMesa = event.detail.value;
    //console.log('Selecccionar MESERO', this.empleadoSeleccionado);
  }

  confirmarCambioMesa() {

    this.stockService.cambioDeMesa(this.selectedLugarConsumo.idLugarFisico, this.lugarCambioMesa.idLugarFisico).then((resulPromise) => {
      resulPromise.subscribe((resul) => {
        //console.log(resul);
        this.stockService.showMessageResponse(resul);
        this.listaPedidos = resul.listEntities;
      });
    });

    this.obtieneMesas();

    this.showCardPedido = false;
    this.showCambioMesa = false;
    this.showMesasPedido = true;
    this.showDatosFactura = false;
    this.lugarConsumoLibre = null;
    this.selectedLugarConsumo = null;

  }

  pagarYFacturar(){

    if (!this.selectedLugarConsumo) {
      this.stockService.showMessageWarning('Debe seleccionar un lugar en la sala');
      return;
    }

    if (this.selectedLugarConsumo.estadoLugar != 'OCUPADO') {
      this.stockService.showMessageWarning('El lugar seleccionado se encuentra libre.');
      return;
    }

    ////buscar por transaccion el total a pagar
    this.listaPedidos = [];
    ///Recuperamos los pedidos anteriores
    this.stockService.transaccionesDetallePorID(this.selectedLugarConsumo.idTransaccion).then((resulPromise) => {
      resulPromise.subscribe((resul) => {
        //console.log(resul);
        //this.stockService.showMessageResponse(resul);
        this.listaPedidos = resul.listEntities;
      });
    });


    this.totalConsumoFinal = 0;

    this.listaPedidos.forEach(x=>{
      this.totalConsumoFinal=this.totalConsumoFinal + (x.precioVenta * x.cantidad);
      this.totalDescuentoFinal=this.totalDescuentoFinal + (x.descuento * x.cantidad); //descuento por producto
    });
    this.formaPagoIni = new FormaPagoDTO();
    this.formaPagoIni.montoACobrar = this.totalConsumoFinal;
    this.formaPagoIni.montoDescuento = this.totalDescuentoFinal;
    //this.formaPagoIni.montoRecibido = 0.00;
    //this.formaPagoIni.
    this.showFormaPago = true;
    this.showCardPedido = false;
    this.showCambioMesa = false;
    this.showMesasPedido = false;
    this.showDatosFactura = false;

  }

  formaDePagoConfirmada(formaPago){
    //console.log('FORMA DE PAGO',formaPago)
    this.formaPago = new FormaPagoDTO();
    this.formaPago = formaPago;
    this.showFormaPago = false;
    this.showCardPedido = false;
    this.showCambioMesa = false;
    this.showMesasPedido = false;
    this.showDatosFactura = true;

  }

  cancelarFormaPago() {

    this.showCardPedido = false;
    this.showCambioMesa = false;
    this.showMesasPedido = true;
    this.lugarConsumoLibre = null;
    this.selectedLugarConsumo = null;
    this.showFormaPago = false;
    this.showDatosFactura = false;
    this.formaPago = null;
  }

  datosFacturaConfirmada(datosFactura){
    //console.log('DATOS FACTURA',datosFactura)
    this.datosFactura = new DatosFacturaDTO();
    this.datosFactura = datosFactura;
    ///Grabar transaccion  venta

  }

  cancelarDatosFactura(){
    this.showCardPedido = false;
    this.showCambioMesa = false;
    this.showMesasPedido = false;
    this.showFormaPago = true;
    this.showDatosFactura = false;
    this.datosFactura = null;
  }

  finalizarTransaccionPago(){
    ///VALIDACIONES
    if (!this.selectedLugarConsumo) {
      this.stockService.showMessageError('Existio un error en la seleccion del lugar de consumo, intente realizar la operación nuevamente');
      this.cancelarFormaPago();
      return;
    }

    if (!this.formaPago)
    {
      this.stockService.showMessageError('Existio un error en la forma de pago, intente realizar la operación nuevamente');
      this.cancelarFormaPago();
      return;
    }

    if (!this.datosFactura)
    {
      this.stockService.showMessageError('Existio un error al obtener datos de la factura, intente realizar la operación nuevamente');
      this.cancelarDatosFactura();
      return;
    }

    ///
    this.transaccionVenta = new TransaccionVentasDTO();
    this.transaccionVenta.idPedMaster = this.selectedLugarConsumo.idTransaccion;
    this.transaccionVenta.idcFormaPago = this.formaPago.idcFormaPago;
    this.transaccionVenta.esEfectivo = this.formaPago.idcFormaPago === 1 ? true:false;
    this.transaccionVenta.nombre = this.datosFactura.nombreFactura;
    this.transaccionVenta.dI = this.datosFactura.documento;
    ///ADICIONAR OTROS DATOS
    this.stockService
      .finalizarPedido(this.transaccionVenta)
      .then(resultPromise => {
        resultPromise.subscribe((resul) => {
          this.stockService.showMessageResponse(resul);

        });
      });
    ///IMPRIMIR FACTURA
    this.obtieneMesas();
    this.transaccionVenta = null;
    this.listaPedidos = null;
    this.empleadoSeleccionado = null;
    this.showCardPedido = false;
    this.showCambioMesa = false;
    this.showMesasPedido = true;
    this.showDatosFactura = false;
    this.formaPago = null;
    this.datosFactura = null;
    this.selectedLugarConsumo = null;
  }

  imprimirComanda(){
    console.log('MESEROOOO',this.empleadoSeleccionado);
    let doc = new DataDocumento();
    doc.titulo = "-----------------------------------";
    doc.titulo = doc.titulo + '\n' +'COMANDA';
    doc.titulo = doc.titulo + '\n' + this.empleadoSeleccionado.nombreCompleto;
    doc.titulo = doc.titulo + '\n' + this.selectedLugarConsumo.descripcion;
    doc.titulo = doc.titulo + '\n' +"-----------------------------------";


    doc.contenido = new Array();
    doc.contenido.push("Cant.|Producto                    |Observación             " + '\n');
    //let listaPedidosImprimir: TransaccionVentasDetalleDTO[] = [];

    for (var i = 0; i < this.listaPedidos.length; i++) {
      //solo los nuevos items se imprimen en la comanda
      if (this.listaPedidos[i].nroPedido < 0 || this.listaPedidos[i]) {
        //listaPedidosImprimir.push(this.listaPedidos[i]);
        doc.contenido.push(this.listaPedidos[i].cantidad.toString() + '|' + this.listaPedidos[i].nombreProducto + '|' + this.listaPedidos[i].observacion + '\n');
      }

    }
    /*
    doc.contenido.push('contenido 1');
    doc.contenido.push('contenido 2');
    doc.contenido.push('contenido 3');
    doc.contenido.push('contenido 4');
    doc.contenido.push('contenido 5');
    */
    console.log('imprimir doccccccc', doc.contenido);

    doc.pie = "-----------------------------------";
    this.documentoService
    .generarDocumentoMultiPlataforma(doc)
    ;
  }
}
