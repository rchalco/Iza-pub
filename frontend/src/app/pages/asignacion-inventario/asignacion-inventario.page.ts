/* eslint-disable @typescript-eslint/member-ordering */
import { Component, OnInit, ViewChild } from '@angular/core';
import { NavController } from '@ionic/angular';
import { ListaProductoComponent } from 'src/app/components/lista-producto/lista-producto.component';
import { DataDocumento } from 'src/app/interfaces/general/documento';
import { InventarioProducto } from 'src/app/interfaces/inventario/InventarioProducto';
import { DocumentoService } from 'src/app/services/documento.service';
import { StockService } from 'src/app/services/stock.service';
import { VentaService } from 'src/app/services/venta.service';
import { InventarioService } from 'src/app/services/inventario.service';

@Component({
  selector: 'app-asignacion-inventario',
  templateUrl: './asignacion-inventario.page.html',
  styleUrls: ['./asignacion-inventario.page.scss'],
})
export class AsignacionInventarioPage implements OnInit {
  barras: any[] = [];
  detalleProductos: InventarioProducto[] = [];
  producto: InventarioProducto;
  barraDestinoObj;

  //selectedProducto: any = null;
  barraSeleccionada = 0;
  barraSeleccionadaOrigen = 0;
  validaCantidades = '';

  @ViewChild('listaProductoComponent')
  listaProductoComponent: ListaProductoComponent;

  constructor(
    private stockService: StockService,
    private ventaService: VentaService,
    private documentoService: DocumentoService,
    private navCtri: NavController,
    private inventarioService: InventarioService
  ) {}

  ngOnInit() {
    this.cargarBarras();
  }

  asignacion() {
    console.log('barra', this.barraSeleccionada);
    if (this.barraSeleccionada === 0) {
      this.ventaService.showMessageWarning(
        'Debe seleccionar el destino de la asignación.'
      );
      return;
    }

    if (this.barraSeleccionadaOrigen === 0) {
      this.ventaService.showMessageWarning(
        'Debe seleccionar el destino de la asignación.'
      );
      return;
    }

    this.detalleProductos = [];
    ///solo los que tiene cantidad mayor a 0
    this.listaProductoComponent.productos.forEach((x) => {
      x.detalle.forEach((yy) => {
        if (yy.cantidad > 0) {
          this.producto = new InventarioProducto();
          this.producto.idProducto = yy.idProducto;
          this.producto.cantidad = yy.cantidad;
          this.detalleProductos.push(this.producto);
        }
      });
    });

    console.log('pedido detalle', this.detalleProductos);
    this.stockService
      .grabaAsignacionProducto(
        this.barraSeleccionadaOrigen,
        this.barraSeleccionada,
        this.detalleProductos
      )
      .then((resul) => {
        resul.subscribe((x) => {
          this.stockService.showMessageResponse(x);
          if (x.state === 1) {
            ///TODO generamos documento
            this.imprimirAsignacion();
            this.listaProductoComponent.resetMontos();
            this.navCtri.navigateRoot('home');
          }
        });
      });
    this.detalleProductos = [];
    //console.log('asignacion');
    //console.log("lista de productos internos", this.listaProductoComponent.productos);
  }

  imprimirAsignacion() {
    console.log('imprimirCompra init');
    const doc = new DataDocumento();
    doc.titulo = '-----------------------------------';
    doc.titulo = doc.titulo + '\n' + 'DETALLE DE ASGINACION';
    //doc.titulo = doc.titulo + '\n' + this.empleadoSeleccionado.nombreCompleto;
    //doc.titulo = doc.titulo + '\n' + this.selectedLugarConsumo.descripcion;
    doc.titulo = doc.titulo + '\n' + '-----------------------------------';

    doc.contenido = new Array();
    doc.contenido.push('ORIGEN: Almacen Central');
    doc.contenido.push('DESTINO: ' + this.barraDestinoObj.descripcion);
    //doc.contenido.push("Cant.|Producto                    |Observación             " + '\n');
    //let listaPedidosImprimir: TransaccionVentasDetalleDTO[] = [];
    let textoLinea = '';
    this.listaProductoComponent.productos.forEach((x) => {
      x.detalle.forEach((yy) => {
        if (yy.cantidad > 0) {
          textoLinea = yy.nombreProducto + ':' + yy.cantidad;
          doc.contenido.push(textoLinea);
        }
      });
    });

    doc.pie = '-----------------------------------';
    doc.pie += '\n\n\n\nFirma Resp. Asignacion';
    doc.pie += '\n\n\n\nFirma Resp. Barra';
    console.log('Documento', doc);
    //android
    // this.documentoService
    //   .generarDocumentoPartial(doc).subscribe(y => {
    //     console.log('generarDocumentoPartial', y);
    //     this.documentoService.writeFilePDF('c:\\tmp\\doc1.pdf', y);
    //   });
    //web
    this.documentoService.generarDocumentoMultiPlataforma(doc);
  }

  cargarBarras() {
   
    this.inventarioService.obtenerAlmacenes(0).then((resul) => {
      resul.subscribe((x) => {
        this.barras = x.listEntities;
        console.log('barras', this.barras);
      });
    });
  }

  selectBarra(event) {
    //this.empleadoSeleccionado = new PersonaResumenDTO();
    this.barraSeleccionada = parseInt(event.detail.value.idAlmacen);
    this.barraDestinoObj = event.detail.value;
    console.log('this.barraDestinoObj', this.barraDestinoObj);
    //this.barraSeleccionada = this.barras.find( ({ idEmpleado }) => idEmpleado === parseInt(event.detail.value) );
    //console.log("barra", this.barraSeleccionada);
  }

  selectBarraOrigen(event) {
    this.barraSeleccionadaOrigen = parseInt(event.detail.value);
    this.listaProductoComponent.cargarProductos(event.detail.value);
  }
}
