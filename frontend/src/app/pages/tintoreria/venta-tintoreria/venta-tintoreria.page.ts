import { Component, OnInit } from '@angular/core';
import { TouchSequence } from 'selenium-webdriver';
import { SmallDocument } from 'src/app/interfaces/printer/smalldocument';
import {
  RequestRegistroPedido,
  TypeDetailPedidoTintoreria,
} from 'src/app/interfaces/tintoreria/RequestRegistroPedido';
import { DetalleVenta } from 'src/app/interfaces/venta/detalleVenta';
import { ResulProductoPrecioVenta } from 'src/app/interfaces/venta/itemProductoVenta';
import { DocumentoService } from 'src/app/services/documento.service';
import { PersonaService } from 'src/app/services/persona.service';
import { StockService } from 'src/app/services/stock.service';
import { TintoreriaService } from 'src/app/services/tintoreria.service';
import { VentaService } from 'src/app/services/venta.service';

@Component({
  selector: 'app-venta-tintoreria',
  templateUrl: './venta-tintoreria.page.html',
  styleUrls: ['./venta-tintoreria.page.scss'],
})
export class VentaTintoreriaPage implements OnInit {
  showCardSearch = true;
  esCaja = false;
  documento: SmallDocument;
  selectedProducto: any = null;
  showCardCliente = false;
  showListClientes = false;
  textoBusacar = '';
  productos: ResulProductoPrecioVenta[] = [];
  productosAvender: any[] = [];
  clienteSelect: any;
  sTotalPrecio: string;
  listaUnidades = [
    {
      nombre: 'Unidad',
      valor: 'UNIDAD',
    },
    {
      nombre: 'Caja',
      valor: 'CAJA',
    },
  ];

  listaClientes: any;

  constructor(
    private tintoreriaService: TintoreriaService,
    private personaService: PersonaService,
    private documentoService: DocumentoService
  ) {}

  ngOnInit() {
    this.cargarProductos();
    this.cargarClientes();
  }

  cargarProductos() {
    this.tintoreriaService.searchProductVenta().then((productosService) => {
      console.log('productosService', productosService);
      productosService.subscribe((resul) => {
        console.log(resul);
        this.productos = resul.listEntities;
        console.log('productos', this.productos);
        this.productos.forEach((x) => {
          x.embase = x.embase ? x.embase.toUpperCase() : 'UNIDAD';
          if (x.picProducto) {
            x.picProducto = 'data:image/jpeg;base64,' + x.picProducto;
          } else {
            x.picProducto = '../../assets/default-tintoreria.png';
          }
        });
      });
    });
  }

  cargarClientes() {
    this.personaService.obtenerClientes().then((personaServiceResul) => {
      personaServiceResul.subscribe((resul) => {
        console.log(resul);
        this.listaClientes = resul.listEntities;
        console.log('clientes', this.listaClientes);
      });
    });
  }

  buscar(event) {
    this.textoBusacar = event.detail.value;
  }

  registroVenta(producto) {
    this.showCardSearch = false;
    this.selectedProducto = producto;
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
    // if (!this.validarCantidadVenta()) {
    //   return;
    // }
    this.productos.forEach((x) => {
      if (x.idProducto === producto.idProducto) {
        x.enStock =
          x.enStock - producto.cantidadVendida * producto.cantidadCaja;
      }
    });
    producto.precioTotal = producto.cantidadVendida * producto.precio;
    producto.secuencialVenta = this.productosAvender.length;
    producto.nombreProducto = producto.nombreProducto;
    this.productosAvender.push(producto);
    this.showCardSearch = true;
    this.selectedProducto = null;
  }

  validarCantidadVenta() {
    console.log('ingrese a validarCantidadVenta');
    let validCantidad = true;
    if (this.selectedProducto.cantidadVendida <= 0) {
      this.tintoreriaService.showMessageWarning(
        'La cantidad de venta debe ser mayor a 0'
      );
      validCantidad = false;
    }

    if (this.selectedProducto.cantidadVendida > this.selectedProducto.enStock) {
      this.tintoreriaService.showMessageWarning(
        'La cantidad de venta no debe ser mayor a ' +
          this.selectedProducto.enStock
      );
      validCantidad = false;
    }
    return validCantidad;
  }

  registrarVentaTotal() {
    if (this.productosAvender.length === 0) {
      this.tintoreriaService.showMessageWarning(
        'Debe tener al menos un producto adicionado para vender'
      );
      return;
    }

    console.log('productosAvender', this.productosAvender);
    let totalPrecio = 0;
    const requestRegistroPedido = new RequestRegistroPedido();
    requestRegistroPedido.idPedMaster = 0;
    requestRegistroPedido.idSesion = 0;
    requestRegistroPedido.idEmpresa = 0;
    requestRegistroPedido.idFacCliente = this.clienteSelect.idclienteFac;
    requestRegistroPedido.detallePedido = [];

    this.productosAvender.forEach((x) => {
      const typeDetailPedidoTintoreria = new TypeDetailPedidoTintoreria();
      typeDetailPedidoTintoreria.idProducto = x.idProducto;
      typeDetailPedidoTintoreria.cantidad = x.cantidadVendida;
      typeDetailPedidoTintoreria.precioFinal = x.precio * x.cantidadVendida;
      requestRegistroPedido.detallePedido.push(typeDetailPedidoTintoreria);
      totalPrecio += typeDetailPedidoTintoreria.precioFinal;
    });

    this.sTotalPrecio = 'Total Bs. ' + totalPrecio.toString();
    console.log('requestRegistroPedido', requestRegistroPedido);
    this.tintoreriaService
      .registrarPedidoTintoreria(requestRegistroPedido)
      .then((registroService) => {
        registroService.subscribe((resul) => {
          this.tintoreriaService.showMessageResponse(resul);

          ///TODO generamos documentos
          if (resul.state === 1) {
            console.log('Se genera el docuemnto');
            this.documento = new SmallDocument();

            this.documento.titulo = 'Mr Jefft - Lavanderia PED' + resul.code;
            this.documento.pie = this.sTotalPrecio;
            this.documento.pathLogo = 'c:\\fonts\\logo-mr-jefft.jpg';
            this.documento.contenido = [];

            this.productosAvender.forEach((zz) => {
              const vPrecioFinal = zz.precio * zz.cantidadVendida;
              this.documento.contenido.push(
                zz.nombreProducto +
                  '... Cant. ' +
                  zz.cantidadVendida.toString() +
                  '. Bs' +
                  vPrecioFinal.toString()
              );
            });
            this.documentoService
              .generarDocumentoPartial(this.documento)
              .subscribe((yyy) => {
                const base64String = btoa(
                  String.fromCharCode(...new Uint8Array(yyy))
                );
                console.log('info del documento generado: ', base64String);
                this.documentoService
                  .writeFilePDF('/vouchers/voucher.pdf', base64String)
                  .then((a) => {
                    this.documentoService.openFilePDF('/vouchers/voucher.pdf');
                  });
              });
          }

          ///TODO limpiamos el panel de ventas
          this.cargarProductos();
          this.productosAvender = [];
        });
      });
  }

  quitarProducto(producto) {
    this.productos.forEach((x) => {
      if (x.idProducto === producto.idProducto) {
        x.enStock =
          x.enStock + producto.cantidadVendida * producto.cantidadCaja;
      }
    });
    this.productosAvender.splice(producto.secuencialVenta, 1);
  }

  buscarCliente(event) {
    // console.log(event.detail.value);
    if (event.detail.value.length > 3) {
      this.textoBusacar = event.detail.value;
      this.showListClientes = true;
    } else {
      this.showListClientes = false;
    }
  }

  seleccionarCliente(cliente) {
    console.log('cliente selccionado', cliente);
    this.clienteSelect = cliente;
    this.showListClientes = false;
    this.textoBusacar = '';
  }

  confirmacionRegistroClienteFac(event) {
    this.showCardCliente = false;
    this.showCardSearch = true;
    this.clienteSelect = new Object();
    this.clienteSelect.documento = event.documento;
    this.clienteSelect.idclienteFac = event.idclienteFac;
    this.clienteSelect.nombreCliente = event.nombreCliente;
  }

  cancelarRegistroClienteFac(event) {
    this.showCardCliente = false;
    this.showCardSearch = true;
  }

  nuvevoCliente() {
    this.showCardCliente = true;
    this.showCardSearch = false;
  }
}
