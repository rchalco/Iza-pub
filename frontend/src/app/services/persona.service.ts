import { URL_MIROVENTAOPERACION } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './baseService';
import { LoadingController, ToastController } from '@ionic/angular';
import {
  environment,
  HEADERS_SERVICE,
  URL_PERSON,
} from 'src/environments/environment';
import { Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { DatabaseService } from './DatabaseService';

const urlPerson = URL_PERSON;
const headers = HEADERS_SERVICE;

@Injectable({
  providedIn: 'root',
})
export class PersonaService extends BaseService {
  constructor(
    public databaseService: DatabaseService,
    public httpClient: HttpClient,
    public loadingController: LoadingController,
    public toastController: ToastController
  ) {
    super(databaseService, httpClient, loadingController, toastController);
  }

  obtenerClientes() {
    const urlQuery = urlPerson + 'ObtenerClientes';

    return this.getInfoEviroment().then((env) => {
      const dataRequest = {
        idEmpresa: env.idEmpresa,
        idSession: env.session,
        documento: '%',
      };

      console.log('datos enviados para buscar productos', dataRequest);

      this.presentLoader();
      return this.httpClient
        .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
        .pipe(
          finalize(() => {
            console.log('**se termino la llamada SearchProduct');
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

  registrarClientreFactura(cliente) {
    const urlQuery = urlPerson + 'RegistrarClientreFactura';

    return this.getInfoEviroment().then((env) => {
      cliente.idEmpresa = env.idEmpresa;
      cliente.idSesion = env.session;

      console.log('cliente a registrar', cliente);

      this.presentLoader();
      return this.httpClient
        .post<any>(urlQuery, JSON.stringify(cliente), { headers })
        .pipe(
          finalize(() => {
            console.log('**se termino la llamada SearchProduct');
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
