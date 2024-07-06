import { Component, OnInit } from '@angular/core';
import { imagenDTO } from 'src/app/interfaces/general/imagen';
import { DatosTarjetaDTO } from 'src/app/interfaces/tarjeta/DatosTarjeta';
import { MovimientoDTO } from 'src/app/interfaces/tarjeta/Movimiento';
import { TarjetaService } from 'src/app/services/tarjeta.service';

@Component({
  selector: 'app-control-tarjetas',
  templateUrl: './control-tarjetas.page.html',
  styleUrls: ['./control-tarjetas.page.scss'],
})
export class ControlTarjetasPage implements OnInit {

  showMain = true;
  textoCIBuscar = '';
  textoPaternoBusacar = '';
  listaTarjetas: DatosTarjetaDTO[] = [];
  listaMovimientos: MovimientoDTO[] = [];

  showModificarTarjeta = false;
  showModificarSaldo = false;
  tarjetaSeleccioada: DatosTarjetaDTO;
  saldoNuevo: 0;
  imagenSubida: imagenDTO;
  procesoFinalizado = false;
  fileToUpload = null;
  constructor(
    private tarjetaService: TarjetaService,
  ) { }

  ngOnInit() {
    this.showMain = true;
    this.showModificarTarjeta = false;
    this.showModificarSaldo = false;
    this.tarjetaSeleccioada = new DatosTarjetaDTO();
  }

  buscar() {
    console.log('buscar CI:' + this.textoCIBuscar);
    this.tarjetaService.filtroClientesTarjeta(this.textoCIBuscar, this.textoPaternoBusacar).then((productosService) => {
      productosService.subscribe((resul) => {
        //console.log(resul);
        //this.productosAgrupados = resul.listEntities;
        //console.log('productos', this.productosAgrupados);
        this.listaTarjetas = resul.listEntities;
        //this.selectedRegistro.picPersonaB64 = 'data:image/jpeg;base64,' + this.selectedRegistro.picPersonaB64;

        this.tarjetaService.showMessageResponse(resul);


      });
    });

    //this.textoCIBusacar = event.detail.value;
  }
  modificarDatos(tarjeta) {
    console.log('Tarjeta:', tarjeta);
    this.tarjetaSeleccioada = new DatosTarjetaDTO();
    this.tarjetaSeleccioada = tarjeta;
    this.showMain = false;
    this.showModificarTarjeta = true;
    this.showModificarSaldo = false;

    this.tarjetaService.verificaTarjeta(this.tarjetaSeleccioada.vkey).then((productosService) => {
      productosService.subscribe((resul) => {
        console.log('Datos....', resul.object);
        this.tarjetaSeleccioada.picPersonaB64 = 'data:image/jpeg;base64,' + resul.object.picPersonaB64;
      });
    });


    //this.tarjetaSeleccioada.picPersonaB64 = 'data:image/jpeg;base64,' + this.tarjetaSeleccioada.picPersonaB64;
    //this.textoCIBusacar = event.detail.value;
  }
  modificarSaldo(tarjeta) {
    console.log('Tarjeta:', tarjeta);
    this.tarjetaSeleccioada = new DatosTarjetaDTO();
    this.tarjetaSeleccioada = tarjeta;
    this.showMain = false;
    this.showModificarTarjeta = false;
    this.showModificarSaldo = true;
    this.saldoNuevo = 0;
    //obtener historico
    this.tarjetaService.movimientosPorTarjeta(this.tarjetaSeleccioada.idCard).then((productosService) => {
      productosService.subscribe((resul) => {
        this.listaMovimientos = resul.listEntities;
        this.tarjetaService.showMessageResponse(resul);
      });
    });
  }

  cancelar() {
    this.tarjetaSeleccioada = new DatosTarjetaDTO();
    this.showMain = true;
    this.showModificarTarjeta = false;
    this.showModificarSaldo = false;
    this.saldoNuevo = 0;

    //this.textoCIBusacar = event.detail.value;
  }

  grabarDatos() {
    this.tarjetaService.grabarTarjeta(this.tarjetaSeleccioada).then((productosService) => {
      productosService.subscribe((resul) => {
        //console.log(resul);
        //this.productosAgrupados = resul.listEntities;
        //console.log('productos', this.productosAgrupados);
        this.tarjetaSeleccioada = resul.object;
        //this.selectedRegistro.picPersonaB64 = 'data:image/jpeg;base64,' + this.selectedRegistro.picPersonaB64;
        this.tarjetaService.showMessageResponse(resul);
        this.cancelar();
        this.listaTarjetas = [];
        this.procesoFinalizado = false;
      });

    });
  }

  grabarSaldo() {

    if (this.saldoNuevo == 0) {
      this.tarjetaService.showMessageWarning('Debe agregar algun monto');
      return;
    }
    let operacion = 0;
    operacion = this.saldoNuevo > 0 ? 1 : 2;
    this.tarjetaService.grabarMovimiento(this.tarjetaSeleccioada.vkey, operacion, this.saldoNuevo).then((productosService) => {
      productosService.subscribe((resul) => {
        this.tarjetaService.showMessageResponse(resul);
        this.cancelar();
        this.listaTarjetas = [];
      });
    });

  }

  handleFileInput(files: FileList) {
    this.imagenSubida = new imagenDTO();
    this.fileToUpload = files.item(0);
    this.tarjetaService.FullPathArchivo(this.fileToUpload).subscribe(x => {
      //console.log('IMAGEN SUBIDA',x);
      this.tarjetaService.showMessageResponse(x);

      this.imagenSubida.fullPath = x.object.fullPath;
      this.imagenSubida.imagen = x.object.imagen;
      this.tarjetaSeleccioada.picPersonaB64 = 'data:image/jpeg;base64,' + this.imagenSubida.imagen;
      this.procesoFinalizado = true;
      //console.log('IMAGEN SUBIDA',this.imagenSubida);
      //this.aperturaAuditoriaService.showMessageSucess("Solcitud cargada correctamente");
    });
    //console.log('IMAGEN SUBIDA',this.imagenSubida);

  }
  selectfechaVigencia(e) { }
}
