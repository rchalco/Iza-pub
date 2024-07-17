/* eslint-disable @typescript-eslint/naming-convention */
import { Component, OnInit, ViewChild } from '@angular/core';
import { ReaderCardComponent } from 'src/app/components/reader-card/reader-card.component';
import { DatosTarjetaDTO } from 'src/app/interfaces/tarjeta/DatosTarjeta';
import { DetalleVenta } from 'src/app/interfaces/venta/detalleVenta';
import { StockService } from 'src/app/services/stock.service';
import { TarjetaService } from 'src/app/services/tarjeta.service';
import { VentaService } from 'src/app/services/venta.service';

@Component({
  selector: 'app-venta-express',
  templateUrl: './venta-express.page.html',
  styleUrls: ['./venta-express.page.scss'],
})
export class VentaExpressPage implements OnInit {
  @ViewChild('appreadercard') cardInput: ReaderCardComponent;

  showCardProductoSelect = false;
  esCaja = false;
  showCobrar = false;
  showPOS = false;
  selectedProducto: any = null;
  showMain = true;
  textoBusacar = '';
  productos = [];
  productosAvender: any[] = [];
  barras: any[] = [];
  idAlmacen = 0;
  totalVenta = 0;
  listDetalleSinStock = [];
  listFormasDePago = [];
  showMessageErrorNOCajaAbierta = false;
  listaDetalleVentas: DetalleVenta[] = [];
  formaPagoActual = 0;
  productsSlides1;
  productsSlides2;
  productsSlides3;

  mensajeTarjeta = 'Esperando Tarjeta...';
  selectedRegistro: DatosTarjetaDTO;
  showButtonVolver = false;
  constructor(
    private ventaService: VentaService,
    private tarjetaService: TarjetaService,
    private stockService: StockService
    ) {}

  ngOnInit() {
    this.showMain = true;
    this.showButtonVolver = false;
    this.formaPagoActual = 0;
    this.ventaService.getInfoEviroment().then((x) => {
      if (x.idAlmacen !== 0 && x.idOperacionDiariaCaja !== 0) {
        this.cargarProductos(x.idAlmacen);
        this.idAlmacen = x.idAlmacen;
      } else {
        this.showMessageErrorNOCajaAbierta = true;
      }
    });
  }
  
  cargarProductos(idBarra) {
    this.ventaService
      .obtieneProductoAlmacen(idBarra)
      .then((productosService) => {
        productosService.subscribe((resul) => {
          console.log(resul);
          this.productos = resul.listEntities;
          this.productsSlides1 = this.productos.filter((hh) => hh.slide === 1);
          this.productsSlides2 = this.productos.filter((hh) => hh.slide === 2);
          this.productsSlides3 = this.productos.filter((hh) => hh.slide === 3);
          console.log('productos', resul);
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

  registroVenta(producto, _idPrecio, _precio, _unidad) {
    //*console.log(producto);
    ////////console.log(this.productosAvender);
    //console.log(producto);
    //console.log(unidad);
    //console.log(idPrecio);
    if (
      this.productosAvender.filter((x) => x.idPrecio === _idPrecio && x.unidad === _unidad).length > 0
    ) {
      const prod = this.productosAvender.filter(
        (x) => x.idPrecio === _idPrecio && x.unidad === _unidad
      )[0];
      prod.cantidadVendida++;
      prod.total = prod.cantidadVendida * _precio;
    } else {
      const newProd  = {idPrecio:  _idPrecio, 
        cantidadVendida: 1, 
        total: _precio, 
        precio: _precio,
        unidad: _unidad,
        nombreProducto: producto.nombreProducto
      };
      // newProd.idPrecio = idPrecio;
      // newProd.cantidadVendida = 1;
      // newProd.total = producto.cantidadVendida * precio;
      // newProd.precio = precio;
      // newProd.unidad = unidad;
      console.log(producto);
      this.selectedProducto = newProd;
      this.productosAvender.push(newProd);
      console.log(this.productosAvender);
      //console.log('producto nuevo');
    }

    
    this.totalVenta += _precio;
    //console.log(this.productosAvender);
  }

  quitarProducto(producto) {
    this.totalVenta -= producto.precio;
    if (producto.cantidadVendida > 1) {
      producto.cantidadVendida--;
      producto.total -= producto.precio;
    } else {
      this.productosAvender = this.productosAvender.filter(
        (x) => x.idPrecio !== producto.idPrecio
      );
    }
  }

  realizarPago(_idFormaPago) {
    if (this.productosAvender.length === 0) {
      this.ventaService.showMessageWarning(
        'Debe seleccionar productos antes de realizar un pago!'
      );
      return;
    }
    if (_idFormaPago === -1) {
      this.showMain = false;
      this.showCobrar = true;
      this.showPOS = false;
      return;
    }

    if (_idFormaPago === 5) {
      this.showMain = false;
      this.showCobrar = false;
      this.showPOS = true;
      this.formaPagoActual = _idFormaPago;
      setTimeout(() => {
        this.cardInput.initReadCard();
      }, 1000);

      return;
    }


    this.listaDetalleVentas = [];
    this.listFormasDePago = [];

    const formaDePago = {
      idPedidoMaestro: 0,
      idFormaPago: _idFormaPago,
      montoCubierto: this.totalVenta,
      Diferencia: 0,
    };
    this.listFormasDePago.push(formaDePago);

    this.productosAvender.forEach((x) => {
      const detalleventaInstance = new DetalleVenta();
      detalleventaInstance.idProducto = x.idProducto;
      detalleventaInstance.idParamPrecio = x.idPrecio;
      detalleventaInstance.cantidad = x.cantidadVendida;
      detalleventaInstance.precioUnitario = x.precio;
      detalleventaInstance.unidadePorCaja = 1;
      detalleventaInstance.precioCaja = 1;
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
          if (resul.state === 1) {
            this.cargarProductos(this.idAlmacen);
            this.productosAvender = [];
            this.listDetalleSinStock = [];
            this.listDetalleSinStock = [];
            this.listaDetalleVentas = [];
            this.totalVenta = 0;
            this.listFormasDePago = [];
          } else if (resul.code === 'SIN_STOCK') {
            this.listDetalleSinStock = resul.listEntities;
            console.log('ingreso al if sin stock', this.listDetalleSinStock);
          }
        });
      });
  }

  realizarPagoMix(plistaFormasDePago) {
    console.log('plistaFormasDePago', plistaFormasDePago);
    this.listaDetalleVentas = [];
    this.listFormasDePago = [];

    plistaFormasDePago.forEach((element) => {
      if (!element.value) {
        element.value = 0;
      }
      const formaDePago = {
        idPedidoMaestro: 0,
        idFormaPago: element.idFormaPago,
        montoCubierto: element.value,
        Diferencia: 0,
      };
      ///TODO para tickets guardamos diferencia
      if (formaDePago.idFormaPago === 3 && formaDePago.montoCubierto > 0) {
        formaDePago.Diferencia = parseFloat(
          (formaDePago.montoCubierto - this.totalVenta).toString()
        );
      }
      this.listFormasDePago.push(formaDePago);
    });

    this.productosAvender.forEach((x) => {
      const detalleventaInstance = new DetalleVenta();
      detalleventaInstance.idProducto = x.idProducto;
      detalleventaInstance.idParamPrecio = x.idPrecio;
      detalleventaInstance.cantidad = x.cantidadVendida;
      detalleventaInstance.precioUnitario = x.precio;
      detalleventaInstance.unidadePorCaja = 1;
      detalleventaInstance.precioCaja = 1;
      this.listaDetalleVentas.push(detalleventaInstance);
    });
    console.log('idAlmacen', this.idAlmacen);

    this.ventaService
      .registrarVenta(
        this.listaDetalleVentas,
        this.listFormasDePago,
        this.idAlmacen, ''
      )
      .then((registroService) => {
        registroService.subscribe((resul) => {
          this.ventaService.showMessageResponse(resul);
          this.showMain = true;

          if (resul.state === 1) {
            this.cargarProductos(this.idAlmacen);
            this.productosAvender = [];
            this.listDetalleSinStock = [];
            this.showCobrar = false;
            this.showPOS = false;
            this.listDetalleSinStock = [];
            this.listaDetalleVentas = [];
            this.totalVenta = 0;
            this.listFormasDePago = [];
          } else if (resul.code === 'SIN_STOCK') {
            this.listDetalleSinStock = resul.listEntities;
            console.log('ingreso al if sin stock', this.listDetalleSinStock);
          }
        });
      });
  }

  cancelarPago() {
    this.showMain = true;
    this.showCobrar = false;
    this.showPOS = false;
    this.showButtonVolver = false;
  }

  reciveTarjeta(tarjeta) {
    console.log('recibi el siguiente numero de tarjeta:' + tarjeta);
    //logica de la lectura de la tarjeta
    this.mensajeTarjeta ='Tarjeta leida';
    this.tarjetaService.verificaTarjetaSimple(tarjeta).then((productosService) => {
      productosService.subscribe((resul) => {
        if (resul.state == 4){
          this.tarjetaService.showMessageWarning(resul.message);
          this.showButtonVolver = true;
          return;
        }
        else
        {
          //this.showLeerTarjeta = false;
          //this.showDatosTarjeta = true;
          //this.showModificarTarjeta = false;
          this.selectedRegistro =  new DatosTarjetaDTO();
          this.selectedRegistro = resul.object;
          console.log('this.selectedRegistro:' + this.selectedRegistro);
          if (this.selectedRegistro.saldo<this.totalVenta)
          {
            this.tarjetaService.showMessageWarning('El saldo de la tarjeta es menor al de la venta');
            this.mensajeTarjeta ='El saldo de la tarjeta es menor a la venta';
            this.showButtonVolver = true;
            setTimeout(() => {
              this.cardInput.initReadCard();
            }, 1000);
            return;
          }
          else
          {

            this.listaDetalleVentas = [];
            this.listFormasDePago = [];

            const formaDePago = {
              idPedidoMaestro: 0,
              idFormaPago: this.formaPagoActual,
              montoCubierto: this.totalVenta,
              Diferencia: 0,
            };
            this.listFormasDePago.push(formaDePago);

            this.productosAvender.forEach((x) => {
              const detalleventaInstance = new DetalleVenta();
              detalleventaInstance.idProducto = x.idProducto;
              detalleventaInstance.idParamPrecio = x.idPrecio;
              detalleventaInstance.cantidad = x.cantidadVendida;
              detalleventaInstance.precioUnitario = x.precio;
              detalleventaInstance.unidadePorCaja = 1;
              detalleventaInstance.precioCaja = 1;
              this.listaDetalleVentas.push(detalleventaInstance);
            });
            console.log('idAlmacen', this.idAlmacen);

            this.ventaService
              .registrarVenta(
                this.listaDetalleVentas,
                this.listFormasDePago,
                this.idAlmacen,
                this.selectedRegistro.vkey
              )
              .then((registroService) => {
                registroService.subscribe((resul) => {
                  this.ventaService.showMessageResponse(resul);
                  if (resul.state === 1) {
                    this.showMain = true;
                    this.showCobrar = false;
                    this.showPOS = false;
                    this.showButtonVolver = false;
                    this.cargarProductos(this.idAlmacen);
                    this.productosAvender = [];
                    this.listDetalleSinStock = [];
                    this.listDetalleSinStock = [];
                    this.listaDetalleVentas = [];
                    this.totalVenta = 0;
                    this.listFormasDePago = [];
                  } else if (resul.code === 'SIN_STOCK') {
                    this.listDetalleSinStock = resul.listEntities;
                    console.log('ingreso al if sin stock', this.listDetalleSinStock);
                  }
                });
              });

          }
        }

      });
    });


  }


}
