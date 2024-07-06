import { URL_MIROVENTAOPERACION } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './baseService';
import { LoadingController, ToastController } from '@ionic/angular';
import {
  environment,
  HEADERS_SERVICE,
  URL_TINTORERIA,
} from 'src/environments/environment';
import { Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { DatabaseService } from './DatabaseService';
import { MDPedidosPorEntregar } from '../interfaces/tintoreria/ResponsePedidosEntregar';

const urlTintoreria = URL_TINTORERIA;
const headers = HEADERS_SERVICE;

@Injectable({
  providedIn: 'root',
})
export class TintoreriaService extends BaseService {
  constructor(
    public databaseService: DatabaseService,
    public httpClient: HttpClient,
    public loadingController: LoadingController,
    public toastController: ToastController
  ) {
    super(databaseService, httpClient, loadingController, toastController);
  }

  searchProductVenta() {
    const urlQuery = urlTintoreria + 'SearchProduct';

    return this.getInfoEviroment().then((env) => {
      const dataRequest = {
        idEmpresa: 1, //environment.idEmpresa,
        idSession: environment.session,
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

  registrarPedidoTintoreria(pDetallePedido) {
    const urlQuery = urlTintoreria + 'RegistrarPedidoTintoreria';

    return this.getInfoEviroment().then((env) => {
      pDetallePedido.idSesion = env.session;
      pDetallePedido.idEmpresa = 1;
      pDetallePedido.idOperacionDiariaCaja = env.idOperacionDiariaCaja;

      console.log('dataRequest', pDetallePedido);
      this.presentLoader();
      return this.httpClient
        .post<any>(urlQuery, JSON.stringify(pDetallePedido), { headers })
        .pipe(
          finalize(() => {
            console.log('**se termino la llamada RegistrarVentas');
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

  obtienePedidosPorEntregar() {
    const urlQuery = urlTintoreria + 'ObtienePedidosPorEntregar';

    return this.getInfoEviroment().then((env) => {
      const dataRequest = {
        idEmpresa: 1, //environment.idEmpresa,
        idSession: environment.session,
      };

      console.log('datos enviados para obtienePedidosPorEntregar', dataRequest);

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

  obtienePedidosReporte(estado: number, fechIni: Date, fecFin: Date) {
    const urlQuery = urlTintoreria + 'ObtienePedidosReporte';

    return this.getInfoEviroment().then((env) => {
      const dataRequest = {
        idEmpresa: 1, //environment.idEmpresa,
        idSession: environment.session,
        idEstado: estado,
        fechaDesde: fechIni.toISOString(),
        fechaHasta: fecFin.toISOString(),
      };

      console.log('datos enviados para obtienePedidosPorEntregar', dataRequest);

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

  obtienePedidosReportes() {
    const urlQuery = urlTintoreria + 'ObtienePedidosPorEntregar';

    return this.getInfoEviroment().then((env) => {
      const dataRequest = {
        idEmpresa: 1, //environment.idEmpresa,
        idSession: environment.session,
      };

      console.log('datos enviados para obtienePedidosPorEntregar', dataRequest);

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

  async obtieneArqueoCaja(pidCaja, fechaInicio, fechaFin) {
    const urlQuery = urlTintoreria + 'ObtieneArqueoCaja';
    const dataRequest = {
      idEmpresa: 1, //environment.idEmpresa,
      idSession: environment.session,
      idCaja: pidCaja,
      fechaDesde: fechaInicio,
      fechaHasta: fechaFin
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.idEmpresa = environment.idEmpresa;
      dataRequest.idSession = environment.session;
    });
    return this.getInfoEviroment().then((env) => {
      console.log('datos enviados para ObtieneArqueoCaja', dataRequest);

      this.presentLoader();
      return this.httpClient
        .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
        .pipe(
          finalize(() => {
            console.log('**se termino la llamada ObtieneArqueoCaja');
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

  async obtieneCajaUsuario() {
    const urlQuery = urlTintoreria + 'ObtieneCajaUsuario';
    const dataRequest = {
      idEmpresa: 1, //environment.idEmpresa,
      idSession: environment.session,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.idEmpresa = environment.idEmpresa;
      dataRequest.idSession = environment.session;
    });
    return this.getInfoEviroment().then((env) => {
      console.log('datos enviados para obtieneCajaUsuario', dataRequest);

      this.presentLoader();
      return this.httpClient
        .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
        .pipe(
          finalize(() => {
            console.log('**se termino la llamada ObtieneArqueoCaja');
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

  entregarPedido(pIdPedidoMaster) {
    const urlQuery = urlTintoreria + 'EntregarPedido';

    return this.getInfoEviroment().then((env) => {
      const dataRequest = {
        idEmpresa: 1, //environment.idEmpresa,
        idSession: environment.session,
        idPedidoMaster: pIdPedidoMaster,
      };

      console.log('datos enviados para entregarPedido', dataRequest);

      this.presentLoader();
      return this.httpClient
        .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
        .pipe(
          finalize(() => {
            console.log('**se termino la llamada entregarPedido');
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
