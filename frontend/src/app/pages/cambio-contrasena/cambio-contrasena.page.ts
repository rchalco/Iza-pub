/* eslint-disable @typescript-eslint/naming-convention */
import { SeguridadService } from './../../services/seguridad.service';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-cambio-contrasena',
  templateUrl: './cambio-contrasena.page.html',
  styleUrls: ['./cambio-contrasena.page.scss'],
})
export class CambioContrasenaPage implements OnInit {
  logidata: any = {
    IdUsuario: environment.idUsuario,
    Usuario: environment.Usuario,
    Password: '',
    PasswordNuevo: '',
    PasswordNuevoRe: '',
    DescripcionError: '',
  };
  validForm = true;
  mensajeError: any = {
    message: 'La contraseña regiditada no es la misma',
    state: 3,
  };
  //validForm = false;
  constructor(private seguridadServices: SeguridadService) {}

  ngOnInit() {}

  cambiarcontrasena(login) {
    //this.logidata = login;
    //console.log('contraseñas', this.logidata.PasswordNuevo);
    //console.log('contraseñas', this.logidata.PasswordNuevoRe);
    this.validForm = true;
    if (this.logidata.PasswordNuevo != this.logidata.PasswordNuevoRe) {
      this.validForm = false;
      //console.log('contraseñas incorrectas',this.mensajeError);
      this.seguridadServices.showMessageResponse(this.mensajeError);
      return;
    }
    this.seguridadServices
      .cambioContrasena(login.Usuario, login.Password, login.PasswordNuevo)
      .subscribe((resul) => {
        this.seguridadServices.showMessageResponse(resul);

        if (resul.state === 3) {
          console.log('error cambio contraseña');
        } else {
          this.logidata.Password = '';
          this.logidata.PasswordNuevo = '';
          this.logidata.PasswordNuevoRe = '';
        }
        //console.log('login lof correcto');

        //console.log('objet', resul);
      });
    //console.log('loginDTO', this.resulLogin);
  }
}
