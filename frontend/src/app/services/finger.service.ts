import { Injectable } from '@angular/core';
import { DatabaseService } from './DatabaseService';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoadingController, ToastController } from '@ionic/angular';
import { BaseService } from './baseService';
import { HEADERS_SERVICE, URL_FINGERS } from 'src/environments/environment';
import { catchError, finalize } from 'rxjs/operators';
import { Observable } from 'rxjs';

const headers = HEADERS_SERVICE;
const urlBiometric = URL_FINGERS;

@Injectable({
  providedIn: 'root'
})
export class FingerService extends BaseService {

  constructor(
    public databaseService: DatabaseService,
    public httpClient: HttpClient,
    public loadingController: LoadingController,
    public toastController: ToastController) {
    super(databaseService, httpClient, loadingController, toastController);
  }
  ///capturar huella para enrolar
  async capturarHuellaEnrrolar(_tiempoDeEspera) {
    let url_query = 'http://localhost:2525/fingerprint/rest/CaptureFingerForEnroll';

    let dataRequest = {
      timeOut: _tiempoDeEspera
    };

    this.presentLoader();

    const param = {};
    console.log('llamando a servicio leer tarjeta');
    this.presentLoader();

    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), {
        headers: new HttpHeaders({ 'content-Type': 'application/json' }),
      })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada verificaTarjeta');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }
  ///capturar huella para verifiacar
  async capturarHuella(_tiempoDeEspera) {
    let url_query = 'http://localhost:2525/fingerprint/rest/CaptureFinger';

    let dataRequest = {
      timeOut: _tiempoDeEspera
    };

    this.presentLoader();

    const param = {};
    console.log('llamando a servicio leer tarjeta');
    this.presentLoader();

    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), {
        headers: new HttpHeaders({ 'content-Type': 'application/json' }),
      })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada capturarHuella');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }


  async enrollHuella(_fingersToEnrroll) {
    let url_query = 'http://localhost:2525/fingerprint/rest/Enroll';

    let dataRequest = {
      typeEnroll: 0,
      fingersToEnrroll: _fingersToEnrroll
    };

    this.presentLoader();

    const param = {};
    console.log('llamando a servicio enroll');
    this.presentLoader();

    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), {
        headers: new HttpHeaders({ 'content-Type': 'application/json' }),
      })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada verificaTarjeta');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  ///capturar huella para verifiacar
  async verifyIdentity(finger) {
    let url_query = URL_FINGERS + 'VerifyIdentity';

    let dataRequest = {
      CI: "",
      FingerCapture: finger
    };

    this.presentLoader();

    const param = {};
    console.log('llamando a servicio leer tarjeta');
    this.presentLoader();

    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), {
        headers: new HttpHeaders({ 'content-Type': 'application/json' }),
      })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada verifyIdentity');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }


}
