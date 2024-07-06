import { Component, OnInit } from '@angular/core';
import { DetalleVenta } from 'src/app/interfaces/venta/detalleVenta';
import { ResulProductoPrecioVenta } from 'src/app/interfaces/venta/itemProductoVenta';
import { StockService } from 'src/app/services/stock.service';
import { VentaService } from 'src/app/services/venta.service';

@Component({
  selector: 'app-venta',
  templateUrl: './venta.page.html',
  styleUrls: ['./venta.page.scss'],
})
export class VentaPage implements OnInit {
  showCardSearch = true;
  showCardProductoSelect = false;
  showStockAgotado = false;
  esCaja = false;
  selectedProducto: any = null;
  textoBusacar = '';
  productos = [];
  productosAvender: any[] = [];
  barras: any[] = [];
  idAlmacen = 0;
  totalVenta = 0;
  listDetalleSinStock = [];
  listFormasDePago = [];
  showCobrar = false;
  showMessageErrorNOCajaAbierta = false;

  listaDetalleVentas: DetalleVenta[] = [];

  constructor(private ventaService: VentaService) {}

  ngOnInit() {
    ///Validamos si tenemos abierta una caja

    this.ventaService.getInfoEviroment().then((x) => {
      if (x.idAlmacen !== 0 && x.idOperacionDiariaCaja !== 0) {
        this.cargarBarras();
        this.cargarProductos(x.idAlmacen);
        this.idAlmacen = x.idAlmacen;
      } else {
        this.showMessageErrorNOCajaAbierta = true;
      }
    });
  }

  verPedido() {
    this.showCardSearch = false;
    this.showCardProductoSelect = true;
  }

  verMenu() {
    this.showCardSearch = true;
    this.showCardProductoSelect = false;
  }

  selectBarra(event) {
    //this.barraCurrent = event.detail.value;
    this.cargarProductos(this.idAlmacen);
  }

  cargarProductos(idBarra) {
    this.ventaService
      .obtieneProductoAlmacen(idBarra)
      .then((productosService) => {
        productosService.subscribe((resul) => {
          console.log(resul);
          this.productos = resul.listEntities;
          console.log('productos', resul);
          this.productos.forEach((x) => {
            x.embase = x.embase ? x.embase.toUpperCase() : 'UNIDAD';
            x.picProducto = 'data:image/jpeg;base64,' + x.picProducto;
          });
        });
      });
  }

  cargarBarras() {
    this.ventaService.obtenerAlmacenes().then((resul) => {
      resul.subscribe((x) => {
        this.barras = x.listEntities;
        console.log('barras', this.barras);
      });
    });
  }

  buscar(event) {
    this.textoBusacar = event.detail.value;
  }

  registroVenta(producto) {
    this.showCardSearch = false;
    this.selectedProducto = producto;
    this.selectedProducto.cantidadVendida = 1;
  }

  selectUnidad(event) {
    console.log('datos seleccionado', event);
    this.esCaja = event.detail.value === 'CAJA' ? true : false;
    this.selectedProducto.unidad = event.detail.value;
  }

  cancelarVenta() {
    this.showCardSearch = true;
    this.selectedProducto = null;
  }

  registrarVenta(producto) {
    if (!this.validarCantidadVenta()) {
      return;
    }
    this.productos.forEach((x) => {
      if (x.idProducto === producto.idProducto) {
        x.enStock =
          x.enStock - producto.cantidadVendida * producto.cantidadCaja;
      }
    });

    producto.precioTotal = producto.cantidadVendida * producto.precio;
    this.totalVenta = this.totalVenta + producto.precioTotal;
    producto.secuencialVenta = this.productosAvender.length;
    this.productosAvender.push(producto);
    this.showCardSearch = true;
    this.selectedProducto = null;
  }

  validarCantidadVenta() {
    console.log('validarCantidadVenta', this.selectedProducto);
    let validCantidad = true;
    if (this.selectedProducto.cantidadVendida <= 0) {
      this.ventaService.showMessageWarning(
        'La cantidad de venta debe ser mayor a 0'
      );
      validCantidad = false;
    }
    return validCantidad;
  }

  registrarVentaTotal() {
    if (this.productosAvender.length === 0) {
      this.ventaService.showMessageWarning(
        'Debe tener al menos un producto adicionado para vender'
      );
      return;
    }
    this.showCardProductoSelect = false;
    this.showCobrar = true;
  }

  quitarProducto(producto) {
    this.totalVenta = 0;
    //console.log('this.productosAvender', this.productosAvender);
    //console.log('producto', producto);
    this.productosAvender.splice(producto.secuencialVenta, 1);

    let contador = 0;

    this.productosAvender.forEach((x) => {
      producto.secuencialVenta = contador;
      this.totalVenta = this.totalVenta + x.precioTotal;
      contador++;
    });
  }

  cancelarPago() {
    this.showCardProductoSelect = true;
    this.showCobrar = false;
    this.showStockAgotado = false;
    this.listDetalleSinStock = [];
  }

  realizarPago(plistaFormasDePago) {
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
        this.idAlmacen,''
      )
      .then((registroService) => {
        registroService.subscribe((resul) => {
          this.ventaService.showMessageResponse(resul);
          if (resul.state === 1) {
            this.cargarProductos(this.idAlmacen);
            this.productosAvender = [];
            this.verMenu();
            this.listDetalleSinStock = [];
            this.showStockAgotado = false;
            this.showCobrar = false;

            this.listDetalleSinStock = [];
            this.listaDetalleVentas = [];
            this.totalVenta = 0;
            this.listFormasDePago = [];
          } else if (resul.code === 'SIN_STOCK') {
            this.listDetalleSinStock = resul.listEntities;
            this.showStockAgotado = true;
            console.log('ingreso al if sin stock', this.listDetalleSinStock);
          }
        });
      });
  }
}
