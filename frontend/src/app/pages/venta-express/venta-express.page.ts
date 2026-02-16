/* eslint-disable @typescript-eslint/naming-convention */
import { Component, OnInit, ViewChild } from '@angular/core';

import { ReaderCardComponent } from 'src/app/components/reader-card/reader-card.component';
import { DatosTarjetaDTO } from 'src/app/interfaces/tarjeta/DatosTarjeta';
import { DetalleVenta } from 'src/app/interfaces/venta/detalleVenta';
import { StockService } from 'src/app/services/stock.service';
import { TarjetaService } from 'src/app/services/tarjeta.service';
import { VentaService } from 'src/app/services/venta.service';

/** Estructura interna de un producto agregado al carrito. */
interface ProductoCarrito {
  idPrecio: number;
  idProducto?: number;
  cantidadVendida: number;
  total: number;
  precio: number;
  unidad: string;
  nombreProducto: string;
}

/** Estructura interna de una forma de pago. */
interface FormaPagoItem {
  idPedidoMaestro: number;
  idFormaPago: number;
  montoCubierto: number;
  Diferencia: number;
}

@Component({
  selector: 'app-venta-express',
  templateUrl: './venta-express.page.html',
  styleUrls: ['./venta-express.page.scss'],
})
export class VentaExpressPage implements OnInit {
  @ViewChild('appreadercard') cardInput: ReaderCardComponent;

  // ── Productos ─────────────────────────────────────────────
  productos: any[] = [];
  productsSlides1: any[] = [];
  productsSlides2: any[] = [];
  productsSlides3: any[] = [];
  productosAvender: ProductoCarrito[] = [];
  selectedProducto: ProductoCarrito = null;
  textoBusacar = '';

  // ── Venta ─────────────────────────────────────────────────
  listaDetalleVentas: DetalleVenta[] = [];
  listFormasDePago: FormaPagoItem[] = [];
  listDetalleSinStock: any[] = [];
  totalVenta = 0;
  formaPagoActual = 0;
  idAlmacen = 0;

  // ── Tarjeta / POS ────────────────────────────────────────
  mensajeTarjeta = 'Esperando Tarjeta...';
  selectedRegistro: DatosTarjetaDTO;

  // ── UI state ──────────────────────────────────────────────
  showMain = true;
  showCobrar = false;
  showPOS = false;
  showCardProductoSelect = false;
  showMessageErrorNOCajaAbierta = false;
  showButtonVolver = false;
  esCaja = false;
  barras: any[] = [];

  constructor(
    private ventaService: VentaService,
    private tarjetaService: TarjetaService,
    private stockService: StockService,
  ) {}

  // ══════════════════════════════════════════════════════════
  //  Lifecycle
  // ══════════════════════════════════════════════════════════

  ngOnInit(): void {
    this.resetUI();
    this.initAlmacen();
  }

  // ══════════════════════════════════════════════════════════
  //  Carga de productos
  // ══════════════════════════════════════════════════════════

  recargarProductos(): void {
    this.initAlmacen();
  }

  cargarProductos(idBarra: number): void {
    this.ventaService
      .obtieneProductoAlmacen(idBarra)
      .then((productosService) => {
        productosService.subscribe((resul) => {
          this.productos = resul.listEntities;
          this.productsSlides1 = this.productos.filter((p) => p.slide === 1);
          this.productsSlides2 = this.productos.filter((p) => p.slide === 2);
          this.productsSlides3 = this.productos.filter((p) => p.slide === 3);

          this.productos.forEach((p) => {
            p.embase = p.embase ? p.embase.toUpperCase() : 'UNIDAD';
            p.picProducto = 'data:image/jpeg;base64,' + p.picProducto;
          });
        });
      });
  }

  // ══════════════════════════════════════════════════════════
  //  Carrito de venta
  // ══════════════════════════════════════════════════════════

  buscar(event: any): void {
    this.textoBusacar = event.detail.value;
  }

  registroVenta(
    producto: any,
    idPrecio: number,
    precio: number,
    unidad: string,
  ): void {
    const existente = this.productosAvender.find(
      (p) => p.idPrecio === idPrecio && p.unidad === unidad,
    );

    if (existente) {
      existente.cantidadVendida++;
      existente.total = existente.cantidadVendida * precio;
    } else {
      const nuevo: ProductoCarrito = {
        idPrecio,
        cantidadVendida: 1,
        total: precio,
        precio,
        unidad,
        nombreProducto: producto.nombreProducto,
      };
      this.selectedProducto = nuevo;
      this.productosAvender.push(nuevo);
    }

    this.totalVenta += precio;
  }

  quitarProducto(producto: ProductoCarrito): void {
    this.totalVenta -= producto.precio;

    if (producto.cantidadVendida > 1) {
      producto.cantidadVendida--;
      producto.total -= producto.precio;
    } else {
      this.productosAvender = this.productosAvender.filter(
        (p) => p.idPrecio !== producto.idPrecio,
      );
    }
  }

  // ══════════════════════════════════════════════════════════
  //  Flujo de pago
  // ══════════════════════════════════════════════════════════

  realizarPago(idFormaPago: number): void {
    if (this.productosAvender.length === 0) {
      this.ventaService.showMessageWarning(
        'Debe seleccionar productos antes de realizar un pago!',
      );
      return;
    }

    // Pago mixto → mostrar pantalla de cobro
    if (idFormaPago === -1) {
      this.mostrarVista('cobrar');
      return;
    }

    // Pago con tarjeta/POS → leer tarjeta
    if (idFormaPago === 5) {
      this.mostrarVista('pos');
      this.formaPagoActual = idFormaPago;
      setTimeout(() => this.cardInput.initReadCard(), 1000);
      return;
    }

    // Pago directo (efectivo, ticket, etc.)
    const formasDePago = [this.crearFormaPago(idFormaPago, this.totalVenta)];
    const detalles = this.buildDetalleVentas();

    this.ejecutarVenta(detalles, formasDePago, '');
  }

  realizarPagoMix(plistaFormasDePago: any[]): void {
    const formasDePago: FormaPagoItem[] = plistaFormasDePago.map((elem) => {
      const monto = elem.value || 0;
      const fp = this.crearFormaPago(elem.idFormaPago, monto);

      // Para tickets: guardar la diferencia
      if (fp.idFormaPago === 3 && fp.montoCubierto > 0) {
        fp.Diferencia = parseFloat(
          (fp.montoCubierto - this.totalVenta).toString(),
        );
      }
      return fp;
    });

    const detalles = this.buildDetalleVentas(true);
    this.ejecutarVenta(detalles, formasDePago, '', true);
  }

  cancelarPago(): void {
    this.resetUI();
  }

  // ══════════════════════════════════════════════════════════
  //  Tarjeta / POS
  // ══════════════════════════════════════════════════════════

  reciveTarjeta(tarjeta: string): void {
    this.mensajeTarjeta = 'Tarjeta leida';

    this.tarjetaService
      .verificaTarjetaSimple(tarjeta)
      .then((productosService) => {
        productosService.subscribe((resul) => {
          if (resul.state === 4) {
            this.tarjetaService.showMessageWarning(resul.message);
            this.showButtonVolver = true;
            return;
          }

          this.selectedRegistro = resul.object as DatosTarjetaDTO;

          if (this.selectedRegistro.saldo < this.totalVenta) {
            this.tarjetaService.showMessageWarning(
              'El saldo de la tarjeta es menor al de la venta',
            );
            this.mensajeTarjeta = 'El saldo de la tarjeta es menor a la venta';
            this.showButtonVolver = true;
            setTimeout(() => this.cardInput.initReadCard(), 1000);
            return;
          }

          // Saldo suficiente → registrar venta con tarjeta
          const formasDePago = [
            this.crearFormaPago(this.formaPagoActual, this.totalVenta),
          ];
          const detalles = this.buildDetalleVentas();
          this.ejecutarVenta(
            detalles,
            formasDePago,
            this.selectedRegistro.vkey,
          );
        });
      });
  }

  // ══════════════════════════════════════════════════════════
  //  Métodos privados
  // ══════════════════════════════════════════════════════════

  /** Valida el almacén y caja desde el environment y carga productos. */
  private initAlmacen(): void {
    this.ventaService.getInfoEviroment().then((env) => {
      if (env.idAlmacen !== 0 && env.idOperacionDiariaCaja !== 0) {
        this.idAlmacen = env.idAlmacen;
        this.cargarProductos(env.idAlmacen);
      } else {
        this.showMessageErrorNOCajaAbierta = true;
      }
    });
  }

  /** Resetea el estado de la UI al estado inicial. */
  private resetUI(): void {
    this.showMain = true;
    this.showCobrar = false;
    this.showPOS = false;
    this.showButtonVolver = false;
    this.formaPagoActual = 0;
  }

  /** Cambia la vista activa: 'main', 'cobrar' o 'pos'. */
  private mostrarVista(vista: 'main' | 'cobrar' | 'pos'): void {
    this.showMain = vista === 'main';
    this.showCobrar = vista === 'cobrar';
    this.showPOS = vista === 'pos';
  }

  /** Crea un objeto de forma de pago. */
  private crearFormaPago(idFormaPago: number, monto: number): FormaPagoItem {
    return {
      idPedidoMaestro: 0,
      idFormaPago,
      montoCubierto: monto,
      Diferencia: 0,
    };
  }

  /** Construye la lista de DetalleVenta a partir del carrito actual. */
  private buildDetalleVentas(incluirPrecioUnitario = false): DetalleVenta[] {
    return this.productosAvender.map((item) => {
      const detalle = new DetalleVenta();
      detalle.idProducto = item.idProducto;
      detalle.idParamPrecio = item.idPrecio;
      detalle.cantidad = item.cantidadVendida;
      detalle.precioFinal = item.precio;
      detalle.unidadePorCaja = 1;
      detalle.precioCaja = 1;
      detalle.nombreProducto = item.nombreProducto;
      if (incluirPrecioUnitario) {
        detalle.precioUnitario = item.precio;
      }
      return detalle;
    });
  }

  /** Ejecuta el registro de venta y limpia el estado en caso de éxito. */
  private ejecutarVenta(
    detalles: DetalleVenta[],
    formasDePago: FormaPagoItem[],
    vkey: string,
    esVistaMixta = false,
  ): void {
    this.ventaService
      .registrarVenta(detalles, formasDePago, this.idAlmacen, vkey)
      .then((registroService) => {
        registroService.subscribe((resul) => {
          this.ventaService.showMessageResponse(resul);

          if (esVistaMixta) {
            this.showMain = true;
          }

          if (resul.state === 1) {
            this.limpiarPostVenta();
            this.ventaService.downLoadFile(resul.code, 'application/pdf');
          } else if (resul.code === 'SIN_STOCK') {
            this.listDetalleSinStock = resul.listEntities;
          }
        });
      });
  }

  /** Limpia todo el estado del carrito y recarga productos. */
  private limpiarPostVenta(): void {
    this.resetUI();
    this.cargarProductos(this.idAlmacen);
    this.productosAvender = [];
    this.listaDetalleVentas = [];
    this.listFormasDePago = [];
    this.listDetalleSinStock = [];
    this.totalVenta = 0;
  }
}
