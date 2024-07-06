import { Component, OnInit } from '@angular/core';
import { FingerService } from 'src/app/services/finger.service';

@Component({
  selector: 'app-verificacion-huella',
  templateUrl: './verificacion-huella.page.html',
  styleUrls: ['./verificacion-huella.page.scss'],
})
export class VerificacionHuellaPage implements OnInit {

  cont = 0;
  hasCaputured = false;
  mensaje1 = '';
  mensaje2 = '';

  interval;
  attemps = 0; //numero de intentos de lectura de tarjeta
  syncTimer = false;
  validating = false;
  loading = false;

  constructor(private fingerService: FingerService) { }

  ngOnInit() {

  }


  capturaHuella() {
    this.fingerService.capturarHuella(6000).then(service => {
      service.subscribe(resul => {
        console.log('resul huella', resul);
        if (resul.CaptureFingerResult.State === 1) {
          this.mensaje1 = "2. Verificando identidad...";
          this.fingerService.verifyIdentity(resul.CaptureFingerResult.Message).then(
            service => {
              service.subscribe(
                resul => {
                  this.mensaje2 = resul.message
                }
              );
            }
          );
        }
        else {
          this.mensaje1 = "2. Huella no capturada intente nuevamente";
        }
      }
      );
    });
  }


}
