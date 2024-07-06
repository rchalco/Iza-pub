import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './baseService';
import { LoadingController, ToastController } from '@ionic/angular';
import {
  environment,
  HEADERS_SERVICE,
  URL_CARDS,
  URL_SECURITY,
} from 'src/environments/environment';
import { Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { DatabaseService } from './DatabaseService';

const urlCard = URL_CARDS;
const urlSeguridad = URL_SECURITY;
const headers = HEADERS_SERVICE;

@Injectable({
  providedIn: 'root'
})
export class TarjetaService extends BaseService {

  constructor(
    public databaseService: DatabaseService,
    public httpClient: HttpClient,
    public loadingController: LoadingController,
    public toastController: ToastController) 
    { 
      super(databaseService, httpClient, loadingController, toastController);
    }

    async verificaTarjeta(_nroTarjeta) {
      let url_query = urlCard + 'VerificaTarjeta';
  
      let dataRequest = {
        ParametroTexto1: _nroTarjeta,
        ParametroLong1: 0,
      };
  
      await this.getInfoEviroment().then((env) => {
        dataRequest.ParametroLong1 = env.session;
      });
  
      this.presentLoader();
      return this.httpClient
        .post<any>(url_query, JSON.stringify(dataRequest), { headers })
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

    async verificaTarjetaSimple(_nroTarjeta) {
      let url_query = urlCard + 'VerificaTarjetaSimple';
  
      let dataRequest = {
        ParametroTexto1: _nroTarjeta,
        ParametroLong1: 0,
      };
  
      await this.getInfoEviroment().then((env) => {
        dataRequest.ParametroLong1 = env.session;
      });
  
      this.presentLoader();
      return this.httpClient
        .post<any>(url_query, JSON.stringify(dataRequest), { headers })
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

    FullPathArchivo(fileToUpload) {
      console.log('FullPathArchivo');
      const urlQuery = urlCard + 'FullPathArchivo';
      this.presentLoader();
      const formData: FormData = new FormData();
      formData.append('fileKey', fileToUpload, fileToUpload.name);
      return this.httpClient
        .post<any>(urlQuery, formData)
        .pipe(
          finalize(() => {
            console.log('se termino la llamada FullPathArchivo');
            this.dismissLoader();
          }),
          catchError((error) => {
            console.error(error);
            this.showMessageError('Error al cargar el archivo ' + error.error);
            return Observable.throw(new Error(error.status));
          })
        );
    }

    async grabarTarjeta(_nroTarjeta) {
      let url_query = urlCard + 'GrabarTarjeta';
  
      let dataRequest = {
        idCard: _nroTarjeta.idCard,
        vkey: _nroTarjeta.vkey,
        idPersona: _nroTarjeta.idPersona,
        documento: _nroTarjeta.documento,
        apellidoPaterno: _nroTarjeta.apellidoPaterno,
        apellidoMaterno: _nroTarjeta.apellidoMaterno,
        nombres: _nroTarjeta.nombres,
        celular: _nroTarjeta.celular,
        email: _nroTarjeta.email,
        saldo: _nroTarjeta.saldo,
        picPersonaB64: _nroTarjeta.picPersonaB64,
        picPersona: _nroTarjeta.picPersona,
        fechaRegistro: _nroTarjeta.fechaRegistro,
        fechaVigencia: _nroTarjeta.fechaVigencia,
        idSesion: 0
       
      };
  
      await this.getInfoEviroment().then((env) => {
        dataRequest.idSesion = env.session;
      });
  
      this.presentLoader();
      return this.httpClient
        .post<any>(url_query, JSON.stringify(dataRequest), { headers })
        .pipe(
          finalize(() => {
            console.log('**se termino la llamada grabarTarjeta');
            this.dismissLoader();
          }),
          catchError((error) => {
            console.error(error);
            this.showMessageError('No se tiene comunicacion con el servidor');
            return Observable.throw(new Error(error.status));
          })
        );
    }

    async filtroClientesTarjeta(_di, _paterno) {
      const urlQuery = urlCard + 'FiltroClientesTarjeta';
      let dataRequest = {
        ParametroTexto1:_di,
        ParametroTexto2:_paterno,
        idSesion:0
      };
  
      await this.getInfoEviroment().then((env) => {
        dataRequest.idSesion = environment.session;
      });
  
      console.log('datos enviados obtieneProductoInventario', dataRequest);
  
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
    }

    async movimientosPorTarjeta(_idCard) {
      const urlQuery = urlCard + 'MovimientosPorTarjeta';
      let dataRequest = {
        ParametroLong1:_idCard,
        idSesion:0
      };
  
      await this.getInfoEviroment().then((env) => {
        dataRequest.idSesion = environment.session;
      });
  
      console.log('datos enviados MovimientosPorTarjeta', dataRequest);
  
      this.presentLoader();
      return this.httpClient
        .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
        .pipe(
          finalize(() => {
            this.dismissLoader();
          }),
          catchError((error) => {
            console.error(error);
            this.showMessageError('No se tiene comunicacion con el servidor');
            return Observable.throw(new Error(error.status));
          })
        );
    }

    async grabarMovimiento(_vKeyCard, _tipoMovimiento, _monto) {
      let url_query = urlCard + 'GrabarMovimiento';
  
      let dataRequest = {
        ParametroInt1: _tipoMovimiento,
        ParametroTexto1: _vKeyCard,
        ParametroDecimal1: _monto,
        ParametroTexto2: '',
      };
  
      await this.getInfoEviroment().then((env) => {
        dataRequest.ParametroTexto2 = env.Usuario;
      });
  
      this.presentLoader();
      return this.httpClient
        .post<any>(url_query, JSON.stringify(dataRequest), { headers })
        .pipe(
          finalize(() => {
            console.log('**se termino la llamada grabarTarjeta');
            this.dismissLoader();
          }),
          catchError((error) => {
            console.error(error);
            this.showMessageError('No se tiene comunicacion con el servidor');
            return Observable.throw(new Error(error.status));
          })
        );
    }
    /////PARA HUELLAS
    async grabarErrolamientoHuella(_clienteHuella) {
      let url_query = urlCard + 'GrabarErrolamientoHuella';
  
      let dataRequest = {
        idCard: _clienteHuella.idCard,
        vkey: _clienteHuella.vkey,
        idPersona: _clienteHuella.idPersona,
        documento: _clienteHuella.documento,
        apellidoPaterno: _clienteHuella.apellidoPaterno,
        apellidoMaterno: _clienteHuella.apellidoMaterno,
        nombres: _clienteHuella.nombres,
        celular: _clienteHuella.celular,
        email: _clienteHuella.email,
        saldo: _clienteHuella.saldo,
        fechaRegistro: _clienteHuella.fechaRegistro,
        fechaVigencia: _clienteHuella.fechaVigencia,
        idSesion: 0,
        huella: _clienteHuella.huella,
        indice: _clienteHuella.indice,
      };
  
      await this.getInfoEviroment().then((env) => {
        dataRequest.idSesion = env.session;
      });
  
      this.presentLoader();
      return this.httpClient
        .post<any>(url_query, JSON.stringify(dataRequest), { headers })
        .pipe(
          finalize(() => {
            console.log('**se termino la llamada grabarTarjeta');
            this.dismissLoader();
          }),
          catchError((error) => {
            console.error(error);
            this.showMessageError('No se tiene comunicacion con el servidor');
            return Observable.throw(new Error(error.status));
          })
        );
    }

    async filtroClientesHuella(_filtro) {
      const urlQuery = urlCard + 'FiltroClientesHuella';
      let dataRequest = {
        ParametroTexto1:_filtro,
        idSesion:0
      };
  
      await this.getInfoEviroment().then((env) => {
        dataRequest.idSesion = environment.session;
      });
  
      console.log('datos enviados obtieneProductoInventario', dataRequest);
  
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
    }


}
