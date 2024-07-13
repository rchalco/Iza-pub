import { SeguridadService } from './../../services/seguridad.service';
/* eslint-disable @typescript-eslint/naming-convention */
import { DatabaseService } from './../../services/DatabaseService';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { StockService } from 'src/app/services/stock.service';
import { environment, verionsApp } from 'src/environments/environment';
import { AppComponent } from 'src/app/app.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {
  logidata: any = {
    IdUsuario: 0,
    Usuario: '',
    Password: '',
    DescripcionError: '',
  };
  resulLogin: any = {
    IdUsuario: 0,
    usuario_vc: '',
    Password: '',
    DescripcionError: '',
    IdOperacionDiariaCaja: 0,
    sesion: 0,
    idCaja: 0,
  };

  version = verionsApp;
  //resulLogin: SaldoCajaDTO ;
  constructor(
    private seguridadServices: SeguridadService,
    private router: Router,
    private databaseService: DatabaseService,
    private appComponent: AppComponent
  ) { }

  ngOnInit() { }
  iniciarsesion(login) {
    this.seguridadServices
      .loginUsuario(login.Usuario, login.Password, this.version)
      .subscribe((resul) => {
        this.resulLogin = resul.data;
        console.log('resulLogin', resul.data);
        if (resul.state === 3) {
          console.log('error login');
          this.seguridadServices.showMessageResponse(resul);
        } else {
          environment.Usuario = this.resulLogin.usuario_vc;
          environment.UsuarioLabel = this.resulLogin.log_respuesta;
          environment.idOperacionDiariaCaja =
            this.resulLogin.idOperacionDiariaCaja;
          environment.session = this.resulLogin.idSesion;
          environment.idCaja = this.resulLogin.idCaja;
          environment.idRol = this.resulLogin.idRol;
          environment.ci = this.resulLogin.ci;
          environment.nombreCompleto = this.resulLogin.nombreCompleto;
          environment.rol = this.resulLogin.rol_name;
          environment.idFechaProceso = this.resulLogin.idFechaProceso;
          environment.fechaProceso = this.resulLogin.fechaProceso;
          environment.idAlmacen = this.resulLogin.idAlmacen;
          this.logidata.Usuario = '';
          this.logidata.Password = '';
          this.databaseService.setItem('enviroment', environment);
          console.log('vamos a home');
          this.appComponent.initMenu();
          this.router.navigateByUrl('home');
          // if (environment.idOperacionDiariaCaja === 0) {
          //   console.log('va a directo a apertura caja');
          //   this.router.navigateByUrl('apertura-caja');
          // } else {
          //   console.log('va a directo a home');
          //   this.router.navigateByUrl('home');
          // }
          
        }
        console.log('usuario', environment);
      });
  }
}
