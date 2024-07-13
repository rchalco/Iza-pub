/* eslint-disable @typescript-eslint/naming-convention */
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './baseService';
import { LoadingController, ToastController } from '@ionic/angular';
import {
  environment,
  HEADERS_SERVICE,
  URL_SECURITY,
} from 'src/environments/environment';
import { Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { DatabaseService } from './DatabaseService';

const urlSeguridad = URL_SECURITY;
const headers = HEADERS_SERVICE;

@Injectable({
  providedIn: 'root',
})
export class SeguridadService extends BaseService {
  constructor(
    public databaseService: DatabaseService,
    public httpClient: HttpClient,
    public loadingController: LoadingController,
    public toastController: ToastController
  ) {
    super(databaseService, httpClient, loadingController, toastController);
  }

  ////SEGURIDAD
  loginUsuario(_usuario, _pass, _version) {
    const urlQuery = urlSeguridad + 'LoginUsuario';

    console.warn('url_query', urlQuery);

    const dataRequest = {
      usuario: _usuario,
      password: _pass,
      passwordNuevo: '',
      idEmpresa: 1,
      version: _version
    };

    this.presentLoader();
    return this.httpClient
      .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada login');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error('error del login', error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  cambioContrasena(_usuario, _pass, _passnuevo) {
    const urlQuery = urlSeguridad + 'CambioContrasena';

    const dataRequest = {
      usuario: _usuario,
      password: _pass,
      passwordNuevo: _passnuevo,
    };

    this.presentLoader();
    return this.httpClient
      .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada cambioContrasena');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async obtieneMenuPorUsuario() {
    const urlQuery = urlSeguridad + 'ObtieneMenuPorUsuario';

     const dataRequest = {
      IdSesion: 0,
      IdRol: 0
     };

     await this.getInfoEviroment().then((env) => {
       dataRequest.IdSesion = environment.session;
       dataRequest.IdRol = environment.idRol;
     });

    //const dataRequest = {};
    console.warn('url_query', urlQuery);
    console.log('ObtieneMenuPorUsuario', dataRequest);

    this.presentLoader();
    return this.httpClient
      .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada login');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error('error del login', error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }


  // obtieneMenuPorUsuario1() {
  //   const urlQuery = urlSeguridad + 'ObtieneMenuPorUsuario';

  //   console.warn('url_query', urlQuery);

  //   const dataRequest = {
  //     ParametroLong1: 0,
  //     ParametroLong2: 0,
  //     ParametroLong3: 0,
  //   };

  //   this.presentLoader();
  //   return this.httpClient
  //     .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
  //     .pipe(
  //       finalize(() => {
  //         console.log('**se termino la llamada login');
  //         this.dismissLoader();
  //       }),
  //       catchError((error) => {
  //         console.error('error del login', error);
  //         this.showMessageError('No se tiene comunicacion con el servidor');
  //         return Observable.throw(new Error(error.status));
  //       })
  //     );
  // }


}
