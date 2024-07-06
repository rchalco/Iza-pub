import { Component, OnInit, ViewChild } from '@angular/core';
import { ReaderCardComponent } from 'src/app/components/reader-card/reader-card.component';
import { imagenDTO } from 'src/app/interfaces/general/imagen';
import { DatosTarjetaDTO } from 'src/app/interfaces/tarjeta/DatosTarjeta';
import { TarjetaService } from 'src/app/services/tarjeta.service';

@Component({
  selector: 'app-tarjeta-read',
  templateUrl: './tarjeta-read.page.html',
  styleUrls: ['./tarjeta-read.page.scss'],
})
export class TarjetaReadPage implements OnInit {

  @ViewChild('appreadercard') cardInput: ReaderCardComponent;
  showDatosTarjeta = false;
  showLeerTarjeta = true;
  showModificarTarjeta = false;
  esTarjetaNueva = true;

  procesoFinalizado = false;
  fileToUpload = null;
  imagenSubida: imagenDTO;
  selectedRegistro: DatosTarjetaDTO;
  fechaRegistroSeleccionada: Date = new Date();
  fechaVigenciaSeleccionada: Date = new Date();


  constructor(
    private tarjetaService: TarjetaService,
  ) { }

  ngOnInit() {


    this.cargaDatosIniciales();

    this.selectedRegistro = new DatosTarjetaDTO();
  }

  cargaDatosIniciales() {
    this.esTarjetaNueva = true;
    setTimeout(() => {
      this.cardInput.initReadCard();
    }, 1000);

  }
  reciveTarjeta(tarjeta) {
    console.log('recibi el siguiente numero de tarjeta:' + tarjeta);

    this.tarjetaService.verificaTarjeta(tarjeta).then((productosService) => {
      productosService.subscribe((resul) => {
        if (resul.state == 4) {
          this.tarjetaService.showMessageWarning(resul.message);
          this.esTarjetaNueva = true;
          this.showLeerTarjeta = false;
          this.showDatosTarjeta = false;
          this.showModificarTarjeta = true;
          this.selectedRegistro = new DatosTarjetaDTO();
          this.selectedRegistro.vkey = tarjeta;

        }
        else {
          this.showLeerTarjeta = false;
          this.showDatosTarjeta = true;
          this.showModificarTarjeta = false;
          this.selectedRegistro = new DatosTarjetaDTO();
          this.selectedRegistro = resul.object;
          if (this.selectedRegistro.idCard === 0)
            this.esTarjetaNueva = true;
          else
            this.esTarjetaNueva = false;

          this.selectedRegistro.picPersonaB64 = 'data:image/jpeg;base64,' + this.selectedRegistro.picPersonaB64;
        }

        console.log(resul);
      });
    });

    setTimeout(() => {
      this.cardInput.initReadCard();
    }, 1000);

  }
  modificar() {
    this.showLeerTarjeta = false;
    this.showDatosTarjeta = false;
    this.showModificarTarjeta = true;

    //this.selectedRegistro = objeto;
  }
  grabar() {
    this.showLeerTarjeta = true;
    this.showDatosTarjeta = false;
    this.showModificarTarjeta = false;

    this.tarjetaService.grabarTarjeta(this.selectedRegistro).then((productosService) => {
      productosService.subscribe((resul) => {
        //console.log(resul);
        //this.productosAgrupados = resul.listEntities;
        //console.log('productos', this.productosAgrupados);
        this.selectedRegistro = resul.object;
        //this.selectedRegistro.picPersonaB64 = 'data:image/jpeg;base64,' + this.selectedRegistro.picPersonaB64;

        this.tarjetaService.showMessageResponse(resul);
        this.cargaDatosIniciales();

        this.procesoFinalizado = false;

        this.showLeerTarjeta = true;
        this.showDatosTarjeta = false;
        this.showModificarTarjeta = false;

      });
    });


  }
  cancelar() {
    this.showLeerTarjeta = true;
    this.showDatosTarjeta = false;
    this.showModificarTarjeta = false;
    setTimeout(() => {
      this.cardInput.initReadCard();
    }, 1000);
  }
  selectfechaRegistro(event) {
    this.fechaRegistroSeleccionada = event;

  }

  selectfechaVigencia(event) {
    this.fechaVigenciaSeleccionada = event;

  }

  handleFileInput(files: FileList) {
    this.imagenSubida = new imagenDTO();
    this.fileToUpload = files.item(0);
    this.tarjetaService.FullPathArchivo(this.fileToUpload).subscribe(x => {
      console.log('IMAGEN SUBIDA', x);
      this.tarjetaService.showMessageResponse(x);

      this.imagenSubida.fullPath = x.object.fullPath;
      this.imagenSubida.imagen = x.object.imagen;
      this.selectedRegistro.picPersonaB64 = 'data:image/jpeg;base64,' + this.imagenSubida.imagen;
      this.procesoFinalizado = true;

      //this.aperturaAuditoriaService.showMessageSucess("Solcitud cargada correctamente");
    });
    console.log('IMAGEN SUBIDA', this.imagenSubida);

  }

  handlePhoto(photoB64) {
    //this.tarjetaService.showMessageWarning('llego a manejador photo: ' + photoB64);
    this.selectedRegistro.picPersonaB64 = photoB64;
  }
}
