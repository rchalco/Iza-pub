import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AppComponent } from 'src/app/app.component';
import { DatabaseService } from 'src/app/services/DatabaseService';
import { SeguridadService } from 'src/app/services/seguridad.service';
import { environment, verionsApp } from 'src/environments/environment';

interface LoginCredentials {
  Usuario: string;
  Password: string;
}

interface LoginResult {
  usuario_vc: string;
  log_respuesta: string;
  idOperacionDiariaCaja: number;
  idSesion: number;
  idCaja: number;
  idRol: number;
  ci: string;
  nombreCompleto: string;
  rol_name: string;
  idFechaProceso: number;
  fechaProceso: string;
  idAlmacen: number;
}

@Component({
  standalone: false,
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit, OnDestroy {
  credentials: LoginCredentials = { Usuario: '', Password: '' };
  isLoading = false;
  readonly version = verionsApp;

  private loginSub?: Subscription;

  constructor(
    private seguridadService: SeguridadService,
    private router: Router,
    private databaseService: DatabaseService,
    private appComponent: AppComponent,
  ) {}

  ngOnInit(): void {
    this.seguridadService.clearMenuCache();
    this.seguridadService.clearSessionData();
    this.databaseService.removeItem('enviroment');
  }

  ngOnDestroy(): void {
    this.loginSub?.unsubscribe();
  }

  iniciarSesion(): void {
    if (this.isLoading) { return; }
    this.isLoading = true;

    this.loginSub = this.seguridadService
      .loginUsuario(this.credentials.Usuario, this.credentials.Password, this.version)
      .subscribe({
        next: (resul) => {
          if (resul.state === 3) {
            this.seguridadService.showMessageResponse(resul);
          } else {
            this.applySession(resul.data as LoginResult);
            this.databaseService.setItem('enviroment', environment);
            this.appComponent.initMenu();
            this.router.navigateByUrl('home', { replaceUrl: true });
          }
          this.credentials = { Usuario: '', Password: '' };
        },
        error: () => { this.isLoading = false; },
        complete: () => { this.isLoading = false; },
      });
  }

  private applySession(data: LoginResult): void {
    environment.Usuario               = data.usuario_vc;
    environment.UsuarioLabel          = data.log_respuesta;
    environment.idOperacionDiariaCaja = data.idOperacionDiariaCaja;
    environment.session               = data.idSesion;
    environment.idCaja                = data.idCaja;
    environment.idRol                 = data.idRol;
    environment.ci                    = data.ci;
    environment.nombreCompleto        = data.nombreCompleto;
    environment.rol                   = data.rol_name;
    environment.idFechaProceso        = data.idFechaProceso;
    environment.fechaProceso          = new Date(data.fechaProceso);
    environment.idAlmacen             = data.idAlmacen;
  }
}
