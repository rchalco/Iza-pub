import { Component, OnInit } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import { ClasificadorDTO } from 'src/app/interfaces/general/clasificador';
import { IngredientesDeMenuGeneralDTO } from 'src/app/interfaces/inventario/IngredientesDeMenuGeneral';
import { ProductoPrecioVenta } from 'src/app/interfaces/inventario/ProductoPrecioVenta';
import { typeIngredientesDeMenu } from 'src/app/interfaces/inventario/typeIngredientesDeMenu';
import { InventarioService } from 'src/app/services/inventario.service';

@Component({
  selector: 'app-configura-menu-ingredientes',
  templateUrl: './configura-menu-ingredientes.page.html',
  styleUrls: ['./configura-menu-ingredientes.page.scss'],
})
export class ConfiguraMenuIngredientesPage implements OnInit {

  listaMenuDetalle: IngredientesDeMenuGeneralDTO[] = [];
  listaMenuMaestro: IngredientesDeMenuGeneralDTO[] = [];
  listaTipoProducto: ClasificadorDTO[] = [];
  selectTipoProducto: ClasificadorDTO = new ClasificadorDTO();

  selectedRegistro: IngredientesDeMenuGeneralDTO = new IngredientesDeMenuGeneralDTO();
  listaDetalleProductoAGrabar: IngredientesDeMenuGeneralDTO[] = [];

  listaIngredientes: IngredientesDeMenuGeneralDTO[] = [];


  dataSourceMenuMaestro: DataSource = new DataSource(this.listaMenuMaestro);
  dataSourceIngredientes: DataSource = new DataSource(this.listaDetalleProductoAGrabar);

  showMain: boolean = true;
  showProducto: boolean = false;
  showIngredientes: boolean = false;

  idTipoProductoSeleccionado: number = 0;

  constructor(private inventarioService: InventarioService) {

    this.modificarIngredientes = this.modificarIngredientes.bind(this);
    this.eliminarIngredientes = this.eliminarIngredientes.bind(this);
  }

  ngOnInit() {
    this.cargarDatosInicialesProducto();
    this.showProducto = false;
    this.inventarioService.ingredientesDeMenuGeneral().then((service) => {
      service.subscribe((response) => {
        this.cargaMenuInicial(response.listEntities);
        /*
        this.listaMenuDetalle = [];
        this.listaMenuDetalle = response.listEntities;
        console.log('DETALLE COMPLETO', this.listaMenuDetalle);
        let idPrecio = 0;
        this.listaMenuDetalle.forEach(x => {
          if (x.idPrecio!= idPrecio) {
            this.listaMenuMaestro.push(x);
          }
          idPrecio = x.idPrecio;
        });
        this.dataSourceMenuMaestro = new DataSource(this.listaMenuMaestro);
        */
      });
    });
  }
  modificarIngredientes(e: any) {
    //console.log(e.row.data);
    let ingrediente = new IngredientesDeMenuGeneralDTO();
    ingrediente = e.row.data;
    this.mostrarIngredientes(ingrediente);
    console.log('modificarIngredientes', e);
  }

  mostrarIngredientes(ingrediente: IngredientesDeMenuGeneralDTO)
  {
    this.showProducto = true;
    this.showMain = false;
    this.selectedRegistro = ingrediente;
    this.selectTipoProducto = new ClasificadorDTO();
    this.selectTipoProducto.idClasificador = ingrediente.idCategoria;
    this.listaDetalleProductoAGrabar = this.listaMenuDetalle.filter(x=>x.idPrecio == ingrediente.idPrecio);
    console.log('Detalle ingredientes', this.listaDetalleProductoAGrabar);
  }

  nuevoMenu() {
    //console.log(e.row.data);

    console.log('Nuevo menu');
    this.showProducto = true;
    this.showMain = false;
    this.selectedRegistro = new IngredientesDeMenuGeneralDTO();
    this.selectTipoProducto = new ClasificadorDTO();
    //this.selectedRegistro.idPrecio = 0;
    this.listaDetalleProductoAGrabar = [];

  }


  tipoProductoSeleccionado(event:any){
    console.log('Tipo Seleccionado',  event.detail);
    this.selectTipoProducto = new ClasificadorDTO();
    this.selectTipoProducto.idClasificador = event.detail.value;
  }


  cargarDatosInicialesProducto(){

    this.inventarioService.clasificadorPorTipo(1).then((productosService) => {
      productosService.subscribe((resul) => {
        console.log('CATEGORIASsssss',resul);
        this.listaTipoProducto = resul.listEntities;

      });
    });
  }
  siguiente(){
    console.log('CATEGORIA',this.selectTipoProducto.idClasificador);
    console.log('PRODUCTO XXXXX',this.selectedRegistro);

    if(this.selectTipoProducto.idClasificador ===  0)
    {
      this.inventarioService.showMessageWarning('Debe ingresar la categoria');
      return;
    }

    if(this.selectedRegistro.descripcionMenu ==  '')
    {
      this.inventarioService.showMessageWarning('Debe ingresar el nombre de producto o menú');
      return;
    }

    if(this.selectedRegistro.marca ==  '')
    {
      this.inventarioService.showMessageWarning('Debe ingresar la Marca');
      return;
    }

    if(this.selectedRegistro.contenido ==  '')
    {
      this.inventarioService.showMessageWarning('Debe ingresar el contenido');
      return;
    }

    if(this.selectedRegistro.embaseXUnidades <=  0)
    {
      this.inventarioService.showMessageWarning('Debe ingresar unidades por embase');
      return;
    }

    if(this.selectedRegistro.precioUnitario <=  0)
    {
      this.inventarioService.showMessageWarning('Debe ingresar el precio unitario');
      return;
    }

    if(this.selectedRegistro.precio <=  0)
    {
      this.inventarioService.showMessageWarning('Debe ingresar el precio de venta');
      return;
    }

    if(!this.selectedRegistro.esParaMenu && !this.selectedRegistro.esProducto)
    {
      this.inventarioService.showMessageWarning('Debe marcar algun item Es Para menu o Es Producto');
      return;
    }


    this.selectedRegistro.idCategoria =  this.selectTipoProducto.idClasificador;
    ///solo ingresa cuando no tiene producto
    if (this.selectedRegistro.idProducto === 0){
      if (this.selectedRegistro.esProducto){
        //console.log('YYYY');
        let ingrediente = new IngredientesDeMenuGeneralDTO();
        ingrediente.idPrecio = 0;
        ingrediente.idProducto =  0;
        ingrediente.idIngrediente = 0;
        ingrediente.nombreProducto = this.selectedRegistro.descripcionMenu;
        ingrediente.medidaUnitaria = this.selectedRegistro.embaseXUnidades;

        //console.log('SIGUIENTE', ingrediente);
        this.listaDetalleProductoAGrabar = [];
        this.listaDetalleProductoAGrabar.push(ingrediente);
      }

    }
    //else
    //{
      ///traer ingredientes de la BD
    //}
    this.showIngredientes = true;
    this.showProducto = false;

  }

  cancelarRegistro() {
    this.showMain = true;
    this.showProducto = false;
    this.showIngredientes = false;
    this.selectedRegistro = new IngredientesDeMenuGeneralDTO();
  }

  adicionarAProducto(producto:any) {
    console.log('PRODUCTO', producto);
    let lista: IngredientesDeMenuGeneralDTO[] = [];
    let productoAdicionar = new IngredientesDeMenuGeneralDTO();
    lista = this.listaDetalleProductoAGrabar.filter(x=>x.idProducto == producto.idProducto);
    //console.log('adicionar', lista);
    if (lista.length > 0) return;

    productoAdicionar.idProducto = producto.idProducto;
    productoAdicionar.nombreProducto = producto.nombreProducto;
    productoAdicionar.precio = producto.precio;
    productoAdicionar.idIngrediente = producto.idIngrediente;
    productoAdicionar.embaseXUnidades =0;
    productoAdicionar.detalle = [];
    productoAdicionar.idPrecio = this.selectedRegistro.idPrecio;
    productoAdicionar.idIngrediente = this.selectedRegistro.idIngrediente;
    this.listaDetalleProductoAGrabar.push(productoAdicionar);
  }

  grabarCompleto(){

    if(this.selectedRegistro.idCategoria ===  0)
    {
      this.inventarioService.showMessageWarning('Debe ingresar la categoria');
      return;
    }

    //Se debe adicionar validaciones
    this.selectedRegistro.detalle = [];
    console.log('DETALLEXXXXX',this.selectedRegistro.detalle);
    let typeDetalle = new typeIngredientesDeMenu();

    this.listaDetalleProductoAGrabar.forEach(x => {
      typeDetalle = new typeIngredientesDeMenu();
      typeDetalle.idProducto = x.idProducto;
      typeDetalle.medidaUnitaria = x.medidaUnitaria;
      typeDetalle.medidaPorcentaje = 0;
      typeDetalle.unidadDeMedida = x.unidaDeMedida;
      this.selectedRegistro.detalle.push(typeDetalle);
    });

    console.log('DETALLEVVVV',this.selectedRegistro.detalle);


    this.inventarioService.grabarMenuGeneralCompleto(this.selectedRegistro).then((service) => {
      service.subscribe((response) => {
        this.cargaMenuInicial(response.listEntities);
      });
    });

    this.showMain = true;
    this.showProducto = false;
    this.showIngredientes = false;
  }

  cargaMenuInicial(lista: IngredientesDeMenuGeneralDTO[]){
    this.listaMenuDetalle = [];
    this.listaMenuDetalle = lista;
    this.listaMenuMaestro = [];
    let idPrecio = 0;
    this.listaMenuDetalle.forEach(x => {
      if (x.idPrecio!= idPrecio) {
        this.listaMenuMaestro.push(x);
      }
      idPrecio = x.idPrecio;
    });
    this.dataSourceMenuMaestro = new DataSource(this.listaMenuMaestro);
    console.log('MAESTROOO',this.listaMenuMaestro);
  }


  cancelarIngrediente() {

    this.showProducto = true;
    this.showMain = false;
    this.showIngredientes = false;

  }

  eliminarIngredientes(e: any) {
    //console.log('ELIMINAR INGRE',e);
    let ingrediente = new IngredientesDeMenuGeneralDTO();
    ingrediente = e.row.data;
    //this.listaDetalleProductoAGrabar

    for (var i = 0; i < this.listaDetalleProductoAGrabar.length; i++) {

      if (this.listaDetalleProductoAGrabar[i] === ingrediente) {

        this.listaDetalleProductoAGrabar.splice(i, 1);
      }

    }

  }


}
