/* eslint-disable @typescript-eslint/naming-convention */
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AlertController, Platform } from '@ionic/angular';
import { Router } from '@angular/router';

import { ReaderCardComponent } from 'src/app/components/reader-card/reader-card.component';
import { PrinterHelper } from 'src/app/helpers/printer.helper';
import { DatosTarjetaDTO } from 'src/app/interfaces/tarjeta/DatosTarjeta';
import { QrPagadoDTO } from 'src/app/interfaces/pagos/QrPagado';
import { DetalleVenta } from 'src/app/interfaces/venta/detalleVenta';
import { FeatureFlagsService } from 'src/app/services/feature-flags.service';
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

/** Forma de pago QR. Debe coincidir con el id que devuelve ObtenerFormasDePago. */
const FORMA_PAGO_QR = 3;

@Component({
  standalone: false,
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

  /** Cantidad deseada por idPrecio antes de agregar al carrito. */
  cantidadesDeseadas = new Map<number, number>();

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
  showQR = false;
  showCardProductoSelect = false;
  showMessageErrorNOCajaAbierta = false;
  showButtonVolver = false;
  showBuscador = false;
  esCaja = false;
  barras: any[] = [];

  constructor(
    private ventaService: VentaService,
    private tarjetaService: TarjetaService,
    private stockService: StockService,
    private featureFlags: FeatureFlagsService,
    private platform: Platform,
    private router: Router,
    private alertController: AlertController,
  ) {}

  // ══════════════════════════════════════════════════════════
  //  Lifecycle
  // ══════════════════════════════════════════════════════════

  ngOnInit(): void {
    this.resetUI();
    this.featureFlags.cargar();
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
          console.log('Productos cargados', this.productos);
        });
      });
  }

  // ══════════════════════════════════════════════════════════
  //  Carrito de venta
  // ══════════════════════════════════════════════════════════

  // ── Stepper de cantidad ───────────────────────────────────

  getCantidad(idPrecio: number): number {
    return this.cantidadesDeseadas.get(idPrecio) ?? 1;
  }

  incrementarCantidad(idPrecio: number): void {
    this.cantidadesDeseadas.set(idPrecio, this.getCantidad(idPrecio) + 1);
  }

  decrementarCantidad(idPrecio: number): void {
    const actual = this.getCantidad(idPrecio);
    if (actual > 1) {
      this.cantidadesDeseadas.set(idPrecio, actual - 1);
    }
  }

  buscar(event: any): void {
    this.textoBusacar = event.detail.value ?? '';
    if (!this.textoBusacar) {
      this.textoBusacar = '';
    }
  }

  limpiarBusqueda(): void {
    this.textoBusacar = '';
  }

  abrirBuscador(): void {
    this.showBuscador = true;
    setTimeout(() => {
      const searchbar = document.querySelector('.buscador-popup ion-searchbar');
      if (searchbar) {
        (searchbar as any).setFocus();
      }
    }, 300);
  }

  cerrarBuscador(): void {
    this.showBuscador = false;
  }

  registroVenta(
    producto: any,
    idPrecio: number,
    precio: number,
    unidad: string,
    cantidad = 1,
  ): void {
    if (this.showBuscador) {
      this.textoBusacar = '';
      this.showBuscador = false;
    }

    const existente = this.productosAvender.find(
      (p) => p.idPrecio === idPrecio && p.unidad === unidad,
    );

    if (existente) {
      existente.cantidadVendida += cantidad;
      existente.total = existente.cantidadVendida * precio;
    } else {
      const nuevo: ProductoCarrito = {
        idPrecio,
        cantidadVendida: cantidad,
        total: precio * cantidad,
        precio,
        unidad,
        nombreProducto: producto.nombreProducto,
      };
      this.selectedProducto = nuevo;
      this.productosAvender.push(nuevo);
    }

    this.totalVenta += precio * cantidad;
    this.cantidadesDeseadas.set(idPrecio, 1);
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

    // Pago por QR → generar el QR de la venta y esperar la acreditacion.
    // Con la feature apagada el boton sigue cobrando, pero como el efectivo: registra la
    // venta al instante. Deshabilitarlo dejaria a la caja sin esa forma de pago.
    if (idFormaPago === FORMA_PAGO_QR && this.featureFlags.pagoQr) {
      this.formaPagoActual = idFormaPago;
      this.mostrarVista('qr');
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
  //  Pago por QR
  // ══════════════════════════════════════════════════════════

  /**
   * El componente detecto un cobro acreditado por el monto de la venta: recien ahi se
   * registra. Antes de la acreditacion no hay venta, porque el QR es de monto editable
   * y el cliente puede pagar de menos o no pagar.
   */
  pagoQrConfirmado(pago: QrPagadoDTO): void {
    console.log('Cobro QR acreditado', pago.qrId, pago.horaPago);

    const formasDePago = [this.crearFormaPago(FORMA_PAGO_QR, this.totalVenta)];
    const detalles = this.buildDetalleVentas();

    // La vista QR se mantiene hasta que la venta quede registrada: si el registro falla,
    // el cajero tiene que ver que la plata ya entro y no volver a cobrar.
    this.ejecutarVenta(detalles, formasDePago, '');
  }

  /** Vuelve al pedido dejando el carrito intacto: el cobro no llego a ocurrir. */
  cancelarPagoQr(): void {
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
    this.showQR = false;
    this.showButtonVolver = false;
    this.formaPagoActual = 0;
  }

  /** Cambia la vista activa: 'main', 'cobrar', 'pos' o 'qr'. */
  private mostrarVista(vista: 'main' | 'cobrar' | 'pos' | 'qr'): void {
    this.showMain = vista === 'main';
    this.showCobrar = vista === 'cobrar';
    this.showPOS = vista === 'pos';
    this.showQR = vista === 'qr';
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
            this.imprimirComprobante(resul.code);
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

  private async imprimirComprobante(base64: string): Promise<void> {
    if (!(this.platform.is('android') || this.platform.is('ios'))) {
      this.abrirPDFEnWeb(base64);
      return;
    }

    const preferredPrinter = PrinterHelper.getPreferredPrinter().address;

    if (!preferredPrinter) {
      this.guardarImpresionPendiente(base64);
      await this.avisarVentaSinImpresion(
        'No hay una impresora configurada en este dispositivo.',
      );
      return;
    }

    try {
      await PrinterHelper.printPdfBase64(base64, preferredPrinter);
    } catch (e) {
      console.error('Error al imprimir comprobante:', e);
      this.guardarImpresionPendiente(base64);
      await this.avisarVentaSinImpresion(
        'No se pudo conectar con la impresora. Verifica que esté encendida y que el Bluetooth esté activo.',
      );
    }
  }

  /**
   * Informa que la venta quedó registrada aunque el comprobante no se imprimió,
   * para evitar que el cajero vuelva a cobrar. Requiere confirmación explícita.
   */
  private async avisarVentaSinImpresion(motivo: string): Promise<void> {
    const alert = await this.alertController.create({
      header: 'Venta registrada',
      subHeader: 'El comprobante no se imprimió',
      message:
        `${motivo} La venta se guardó correctamente: no vuelvas a cobrarla. ` +
        'El comprobante quedó pendiente y se imprimirá al configurar la impresora.',
      backdropDismiss: false,
      buttons: [
        { text: 'Seguir vendiendo', role: 'cancel' },
        {
          text: 'Configurar impresora',
          handler: () => {
            this.router.navigate(['/config-printer']);
          },
        },
      ],
    });
    await alert.present();
  }

  private guardarImpresionPendiente(base64: string): void {
    PrinterHelper.savePendingSalePrint(base64);
  }

  /** Abre el PDF base64 como blob en una nueva pestaña (web/fallback). */
  private abrirPDFEnWeb(base64: string): void {
    const binaryData = atob(base64);
    const byteArray = new Uint8Array(binaryData.length);
    for (let i = 0; i < binaryData.length; i++) {
      byteArray[i] = binaryData.charCodeAt(i);
    }
    const blob = new Blob([byteArray], { type: 'application/pdf' });
    const url = window.URL.createObjectURL(blob);
    window.open(url);
  }
}
