import { Component, OnInit, ViewChild } from '@angular/core';
import { ComponentsModule } from 'src/app/components/components.module';
import { DataDocumento } from 'src/app/interfaces/general/documento';
import { InventarioProducto } from 'src/app/interfaces/inventario/InventarioProducto';
import { DocumentoService } from 'src/app/services/documento.service';
import { StockService } from 'src/app/services/stock.service';
import { VentaService } from 'src/app/services/venta.service';

@Component({
  selector: 'app-inventario-general',
  templateUrl: './inventario-general.page.html',
  styleUrls: ['./inventario-general.page.scss'],
})
export class InventarioGeneralPage implements OnInit {
  showCardSearch = true;
  showCardProductoSelect = false;
  productos = [];
  productosAvender: any[] = [];
  selectedProducto: any = null;
  listaDetalleAsignacion: InventarioProducto[] = [];
  tipoCurrent;
  showCantidadCaja = false;
  tipoProducto = [
    {
      idTipo: 1,
      descripcion: 'UNIDAD',
    },
    {
      idTipo: 2,
      descripcion: 'CAJA',
    },
  ];

  descripcionCantidad;
  totalCompra = 0;

  constructor(
    private stockService: StockService,
    private documentoService: DocumentoService
  ) { }

  ngOnInit() {
    //console.log('loaddddddd')
    this.cargarProductos();
    this.descripcionCantidad = 'Cantidad';
  }

  grabarInventario() { }

  asignacionInventario(producto) {
    this.showCardSearch = false;
    this.selectedProducto = producto;
  }

  verPedido() {
    this.showCardSearch = false;
    this.showCardProductoSelect = true;
  }

  verMenu() {
    this.showCardSearch = true;
    this.showCardProductoSelect = false;
  }
  registrarCompraTotal() {
    if (this.productosAvender.length === 0) {
      this.stockService.showMessageWarning(
        'Debe tener al menos un producto adicionado'
      );
    }
    this.listaDetalleAsignacion = [];

    this.productosAvender.forEach((x) => {
      const detalleAsignaInstance = new InventarioProducto();
      detalleAsignaInstance.idProducto = x.idProducto;
      if (x.idTipo === 1) {
        detalleAsignaInstance.cantidad = x.cantidadVendida;
        detalleAsignaInstance.montoCompra =
          Math.round((x.precio / x.cantidadVendida) * 100) / 100;
        detalleAsignaInstance.cantidadCaja = 0;
      } else {
        detalleAsignaInstance.cantidad = x.cantidadCaja * x.cantidadVendida;
        detalleAsignaInstance.montoCompra =
          Math.round((x.precio / (x.cantidadCaja * x.cantidadVendida)) * 100) /
          100;
        detalleAsignaInstance.cantidadCaja = x.cantidadCaja;
      }
      detalleAsignaInstance.idTipo = x.idTipo;
      detalleAsignaInstance.montoTotal = x.precio;
      detalleAsignaInstance.nombreProducto = x.nombreProducto;

      //detalleAsignaInstance.montoCompra = 1;
      this.listaDetalleAsignacion.push(detalleAsignaInstance);
    });
    console.log(
      'Productos a vender',
      this.listaDetalleAsignacion,
      this.productosAvender
    );
    this.stockService
      .grabaAsignacionCompra(this.listaDetalleAsignacion)
      .then((resul) => {
        resul.subscribe((x) => {
          this.stockService.showMessageResponse(x);
          if (x.state === 1) {
            this.imprimirCompra();
            this.cancelarVenta();
            this.cargarProductos();
            this.productosAvender = [];
            this.listaDetalleAsignacion = [];
            this.selectedProducto = null;
            this.totalCompra = 0;
            this.showCardProductoSelect = false;
          }
        });
      });
  }

  registrarCompra(producto) {
    if (!this.validarCantidadAsigna()) {
      return;
    }
    this.productos.forEach((x) => {
      if (x.idProducto === producto.idProducto) {
        x.enStock =
          x.enStock - producto.cantidadVendida * producto.cantidadCaja;
      }
    });
    console.log('producto', producto);
    producto.idTipo = this.tipoCurrent;
    producto.precioTotal = producto.precio;
    this.totalCompra = this.totalCompra + producto.precio;
    producto.secuencialVenta = this.productosAvender.length;
    this.productosAvender.push(producto);
    this.showCardSearch = true;
    this.selectedProducto = null;
  }

  validarCantidadAsigna() {
    //console.log('validarCantidadVenta', this.selectedProducto);
    let validCantidad = true;
    if (this.selectedProducto.cantidadVendida <= 0) {
      this.stockService.showMessageWarning(
        'La cantidad de comrpra debe ser mayor a 0'
      );
      validCantidad = false;
    }
    return validCantidad;
  }

  cargarProductos() {
    this.stockService.obtieneProductosCentral().then((productosService) => {
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
  cancelarVenta() {
    this.showCardSearch = true;
    this.selectedProducto = null;
  }

  selectTipo(event) {
    this.tipoCurrent = event.detail.value;
    if (this.tipoCurrent === 2) {
      this.showCantidadCaja = true;
      this.descripcionCantidad = 'Cantidad de Cajas';
    } else {
      this.showCantidadCaja = false;
      this.descripcionCantidad = 'Cantidad';
    }

    //this.showCantidadCaja = false;
    console.log('Tipo', this.showCantidadCaja);
  }

  quitarProducto(producto) {
    this.totalCompra -= producto.precioTotal;
    console.log('this.productosAvender', this.productosAvender);
    console.log('producto', producto);
    this.productosAvender = this.productosAvender.filter(
      (x) => x.idProducto !== producto.idProducto
    );
  }

  imprimirCompra() {
    console.log('imprimirCompra init');
    const doc = new DataDocumento();
    doc.titulo = '-----------------------------------';
    doc.titulo = doc.titulo + '\n' + 'DETALLE DE COMPRA';
    doc.titulo = doc.titulo + '\n' + '-----------------------------------';

    doc.contenido = new Array();

    let textoLinea = '';
    this.listaDetalleAsignacion.forEach((x) => {
      if (x.cantidadCaja > 0) {
        textoLinea =
          textoLinea +
          'Prod: ' +
          x.nombreProducto +
          '\n' +
          'Cjas: ' +
          x.cantidadCaja.toString() +
          ' Unid: ' +
          x.cantidad.toString() +
          ' Total ' +
          x.montoTotal.toString() +
          '\n';
      } else {
        textoLinea =
          textoLinea +
          'Prod: ' +
          x.nombreProducto +
          '\n' +
          ' Unid: ' +
          x.cantidad.toString() +
          ' Total ' +
          x.montoTotal.toString() +
          '\n';
      }

      textoLinea = textoLinea + '-----------------------------------\n';
      doc.contenido.push(textoLinea);
      textoLinea = '';
    });

    doc.pie = '-----------------------------------';
    doc.pie += '\n\n\n\nFirma Proveedor';
    doc.pie += '\n\n\n\nFirma Resp. Compra\n';
    console.log('Documento', doc);
    this.documentoService.generarDocumentoMultiPlataforma(doc);
  }
}
