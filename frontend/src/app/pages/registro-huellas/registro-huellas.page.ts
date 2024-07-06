import { Component, OnInit } from '@angular/core';
import { DatosTarjetaDTO } from 'src/app/interfaces/tarjeta/DatosTarjeta';
import { MovimientoDTO } from 'src/app/interfaces/tarjeta/Movimiento';
import { FingerService } from 'src/app/services/finger.service';
import { TarjetaService } from 'src/app/services/tarjeta.service';

@Component({
  selector: 'app-registro-huellas',
  templateUrl: './registro-huellas.page.html',
  styleUrls: ['./registro-huellas.page.scss'],
})
export class RegistroHuellasPage implements OnInit {

  showMain = true;
  showModificarCliente = false;

  textoCIBuscar = '';
  textoPaternoBusacar = '';

  procesoFinalizado = false;
  showHuella25 = false;
  showHuella50 = false;
  showHuella75 = false;
  showHuella100 = false;

  contadorErolar = 0;
  huellasCapturadas = [];
  huella = "";
  huellaEnrolada = "";

  botonEnrolarVisible = false;
  botonCapturarVisible = true;

  listaClientesHuella: DatosTarjetaDTO[] = [];
  listaMovimientos: MovimientoDTO[] = [];
  clienteSeleccionado: DatosTarjetaDTO;

  tipoHuellaSeleccionada: number = 0;


  constructor(
    private fingerService: FingerService,
    private tarjetaService: TarjetaService,
  ) { }

  ngOnInit() {
    /*
    const cliente= new DatosTarjetaDTO();
    cliente.idPersona = 0;
    cliente.apellidoPaterno = 'Chalar';
    cliente.apellidoMaterno = 'Escalante';
    cliente.nombres = 'Ruben Miguel';
    cliente.documento = '4010898';
    this.listaClientesHuella.push(cliente);
    */
    this.contadorErolar = 0;
  }

  buscar() {
    console.log('buscar CI:' + this.textoCIBuscar);
    this.tarjetaService.filtroClientesHuella(this.textoCIBuscar).then((productosService) => {
      productosService.subscribe((resul) => {
        this.listaClientesHuella = resul.listEntities;
        this.tarjetaService.showMessageResponse(resul);
      });
    });

  }

  modificarDatos(tarjeta) {
    console.log('Tarjeta:', tarjeta);
    this.clienteSeleccionado = new DatosTarjetaDTO();
    this.clienteSeleccionado = tarjeta;
    this.showMain = false;
    this.showModificarCliente = true;

    // this.fingerService.verificaTarjeta(this.tarjetaSeleccioada.vkey).then((productosService) => {
    //   productosService.subscribe((resul) => {
    //     console.log('Datos....', resul.object);
    //     this.clienteSeleccionado.picPersonaB64 = 'data:image/jpeg;base64,' + resul.object.picPersonaB64;
    //   });
    // });
  }

  cancelar() {
    this.clienteSeleccionado = new DatosTarjetaDTO();
    this.showMain = true;
    this.showModificarCliente = false;
  }

  grabarDatos() {
    if (this.huellaEnrolada === '') {
      this.tarjetaService.showMessageWarning('Debe enrrolar alguna huella');
      return;
    }
    this.clienteSeleccionado.indice = this.tipoHuellaSeleccionada;
    this.clienteSeleccionado.huella = this.huellaEnrolada;
    //console.log('cliente a grabar', this.clienteSeleccionado);
    this.tarjetaService.grabarErrolamientoHuella(this.clienteSeleccionado).then((productosService) => {
      productosService.subscribe((resul) => {
        console.log('resultador', resul);
        this.clienteSeleccionado.idPersona = resul.object.idPersona;
        this.tarjetaService.showMessageResponse(resul);
        this.huellaEnrolada = '';
        this.huellasCapturadas = [];
      });
    });
  }

  setHuella(value) {
    this.botonEnrolarVisible = false;

    this.huella = value;
    console.log('Huelllaaaaaaa', value);
    if (value === 'Huella no capturada') return;
    this.contadorErolar++;
    switch (this.contadorErolar) {
      case 1: {
        this.showHuella25 = true;
        this.showHuella50 = false;
        this.showHuella75 = false;
        this.showHuella100 = false;
        this.huellasCapturadas = [];
        this.huellasCapturadas.push(value);
        break;
      }
      case 2: {
        this.showHuella25 = false;
        this.showHuella50 = true;
        this.showHuella75 = false;
        this.showHuella100 = false;
        this.huellasCapturadas.push(value);
        break;
      }
      case 3: {
        this.showHuella25 = false;
        this.showHuella50 = false;
        this.showHuella75 = true;
        this.showHuella100 = false;
        this.huellasCapturadas.push(value);
        break;
      }
      case 4: {
        this.showHuella25 = false;
        this.showHuella50 = false;
        this.showHuella75 = false;
        this.showHuella100 = true;
        this.huellasCapturadas.push(value);
        this.botonEnrolarVisible = true;
        this.botonCapturarVisible = false;
        break;
      }


      default: {

        //statements; 
        break;
      }
    }
  }

  setTipoHuella(tipoHuella: any) {

    this.tipoHuellaSeleccionada = tipoHuella.detail.value;
    this.huellasCapturadas = [];
    this.contadorErolar = 0;
    this.showHuella25 = false;
    this.showHuella50 = false;
    this.showHuella75 = false;
    this.showHuella100 = false;
  }

  enrolarHuella() {

    this.huellaEnrolada = '';
    //console.log('lista de huellas', this.huellasCapturadas);
    this.fingerService.enrollHuella(this.huellasCapturadas).then(service => {
      service.subscribe(resul => {

        this.huellaEnrolada = resul.EnrollResult.Message;
        console.log('resultado enroll', this.huellaEnrolada);
        this.grabarDatos();
      });
    });

    this.botonEnrolarVisible = false;
    this.botonCapturarVisible = true;
    this.showHuella25 = false;
    this.showHuella50 = false;
    this.showHuella75 = false;
    this.showHuella100 = false;
  }
  nuevoCliente() {
    this.huellaEnrolada = '';
    this.clienteSeleccionado = new DatosTarjetaDTO();
    this.botonEnrolarVisible = false;
    this.showMain = false;
    this.showModificarCliente = true;
  }
  selectfechaVigencia(event) { }


}
