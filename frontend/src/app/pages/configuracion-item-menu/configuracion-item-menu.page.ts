import { Component, OnInit } from '@angular/core';
import { FabulaService } from 'src/app/services/fabula.service';

@Component({
  selector: 'app-configuracion-item-menu',
  templateUrl: './configuracion-item-menu.page.html',
  styleUrls: ['./configuracion-item-menu.page.scss'],
})
export class ConfiguracionItemMenuPage implements OnInit {

  itemsMenus: any[] = [];
  productos: any[] = [];
  textoBuscar = '';
  textoBuscarProducto = '';
  showCardSearch = true;
  totalItemsMenu = 0;
  currentItem: any;
  currentProducto: any;
  showProductoLista = false;
  factor = 0;
  tipoFactor = 0;

  constructor(private fabulaService: FabulaService) { }

  ngOnInit() {
    this.cargarItemsMenu();
    this.cargarProductos();
  }

  cargarItemsMenu() {
    this.fabulaService.obtieneMenuItemsConfiguracion().then((resulPromise) => {
      resulPromise.subscribe((resul) => {
        this.itemsMenus = resul.listEntities;
        this.totalItemsMenu = this.itemsMenus.length;
      });
    });
  }

  cargarProductos() {
    this.fabulaService.obtieneProductos().then((resulPromise) => {
      resulPromise.subscribe((resul) => {
        this.productos = resul.listEntities;
      });
    });
  }

  buscar(event) {
    this.textoBuscar = event.detail.value;
  }

  buscarProducto(event) {
    this.textoBuscarProducto = event.detail.value;
  }

  verConfiguracion(_item) {
    this.currentItem = _item;
    this.showCardSearch = false;
  }

  seleccionarProducto(_producto) {
    this.showProductoLista = false;
    this.currentProducto = _producto;
  }

  addProductoItem() {
    this.showProductoLista = true;
  }

  setValueFactor(event) {
    this.factor = event.detail.value;
  }

  registrarConfiguracion() {
    const composicion = {
      idComposicionParamPrecio: 0,
      idProducto: this.currentProducto.idProducto,
      idParamPrecio: this.currentItem.idPrecio,
      nombreProducto: this.currentProducto.nombreProducto,
      marca: this.currentProducto.marca,
      contenido: this.currentProducto.contenido,
      descripcion: this.currentProducto.descripcion,
      factor: this.tipoFactor === 0 ? this.factor : this.factor / 100,
      operacion: 'add'
    };

    this.fabulaService.grabarComposicion(composicion).then(
      resulPromise => {
        resulPromise.subscribe(resul => {
          console.log(resul);
          composicion.idComposicionParamPrecio = resul.code;
          this.fabulaService.showMessageResponse(resul);
          this.currentItem.configuracion.push(composicion);
          this.showProductoLista = false;
          this.currentProducto = null;
          this.factor = 0;
        });
      }
    );
  }
  eliminarComposicion(composicion) {
    console.log('eliminar', composicion);
    composicion.operacion = 'del';
    this.fabulaService.grabarComposicion(composicion).then(
      resulPromise => {
        resulPromise.subscribe(resul => {
          console.log(resul);
          composicion.idComposicionParamPrecio = resul.code;
          this.fabulaService.showMessageResponse(resul);
          this.currentItem.configuracion = this.currentItem.configuracion
            .filter((item) => item.idComposicionParamPrecio !== composicion.idComposicionParamPrecio);
          this.showProductoLista = false;
          this.currentProducto = null;
          this.factor = 0;
        });
      }
    );
  }
  volver() {
    this.showCardSearch = true;
    this.currentItem = null;
  }

  selectTipoFactor(value) {
    this.tipoFactor = value;
  }
}
