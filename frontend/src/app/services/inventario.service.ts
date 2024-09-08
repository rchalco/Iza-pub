/* eslint-disable @typescript-eslint/naming-convention */
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './baseService';
import { LoadingController, ToastController } from '@ionic/angular';
import {
  environment,
  HEADERS_SERVICE,
  URL_INVENTARIO,
} from 'src/environments/environment';
import { Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { DatabaseService } from './DatabaseService';
import { InventarioAsignacion } from '../interfaces/inventario/InventarioAsignacion';

const urlInventario = URL_INVENTARIO;
const headers = HEADERS_SERVICE;

@Injectable({
  providedIn: 'root',
})
export class InventarioService extends BaseService {
  constructor(
    public databaseService: DatabaseService,
    public httpClient: HttpClient,
    public loadingController: LoadingController,
    public toastController: ToastController
  ) {
    super(databaseService, httpClient, loadingController, toastController);
  }

  async obtenerAlmacenes() {
    const urlQuery = urlInventario + 'SolicitarAmbientes';

    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
    });


    return this.getInfoEviroment().then((env) => {
      const dataRequest = {};
      this.presentLoader();
      return this.httpClient
        .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
        .pipe(
          finalize(() => {
            console.log('**se termino la llamada obtenerAlmacenes');
            this.dismissLoader();
          }),
          catchError((error) => {
            console.error(error);
            this.showMessageError('No se tiene comunicacion con el servidor');
            return Observable.throw(new Error(error.status));
          })
        );
    });
  }

  async ObtenerProductosAlmacenCentral() {
    const urlQuery = urlInventario + 'ObtenerProductosAlmacenCentral';

    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
    });


    return this.getInfoEviroment().then((env) => {
      const dataRequest = {};
      this.presentLoader();
      return this.httpClient
        .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
        .pipe(
          finalize(() => {
            console.log('**se termino la llamada ObtenerProductosAlmacenCentral');
            this.dismissLoader();
          }),
          catchError((error) => {
            console.error(error);
            this.showMessageError('No se tiene comunicacion con el servidor');
            return Observable.throw(new Error(error.status));
          })
        );
    });
  }

  async obtenerDashboardProductos() {
    const urlQuery = urlInventario + 'ObtenerDashboardProductos';

    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
    });

    return this.getInfoEviroment().then((env) => {
      const dataRequest = {};
      this.presentLoader();
      return this.httpClient
        .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
        .pipe(
          finalize(() => {
            console.log('**se termino la llamada obtenerAlmacenes');
            this.dismissLoader();
          }),
          catchError((error) => {
            console.error(error);
            this.showMessageError('No se tiene comunicacion con el servidor');
            return Observable.throw(new Error(error.status));
          })
        );
    });
  }

  async obtenerAlmacenesParaAsignacion() {
    const urlQuery = urlInventario + 'SolicitarAmbientesCompleto';

    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
    });


    return this.getInfoEviroment().then((env) => {
      const dataRequest = {};
      this.presentLoader();
      return this.httpClient
        .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
        .pipe(
          finalize(() => {
            console.log('**se termino la llamada obtenerAlmacenes');
            this.dismissLoader();
          }),
          catchError((error) => {
            console.error(error);
            this.showMessageError('No se tiene comunicacion con el servidor');
            return Observable.throw(new Error(error.status));
          })
        );
    });
  }

  async GrabaAsignacionProducto(inventarioAsignacion: InventarioAsignacion) {
    const urlQuery = urlInventario + 'GrabaAsignacionProducto';
    await this.getInfoEviroment().then((env) => {
      inventarioAsignacion.idSesion = env.session;
      inventarioAsignacion.idfechaproceso = env.idFechaProceso;
      inventarioAsignacion.observaciones = env.Usuario;
      inventarioAsignacion.idOperacionDiariaCaja = env.idOperacionDiaria;
    });
    return this.getInfoEviroment().then((env) => {      
      this.presentLoader();
      return this.httpClient
        .post<any>(urlQuery, JSON.stringify(inventarioAsignacion), { headers })
        .pipe(
          finalize(() => {
            console.log('**se termino la llamada obtenerAlmacenes');
            this.dismissLoader();
          }),
          catchError((error) => {
            console.error(error);
            this.showMessageError('No se tiene comunicacion con el servidor');
            return Observable.throw(new Error(error.status));
          })
        );
    });
  }
}
