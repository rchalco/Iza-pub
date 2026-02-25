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
import { IngredientesDeMenuGeneralDTO } from '../interfaces/inventario/IngredientesDeMenuGeneral';

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

  async obtenerAlmacenes(_tipoAlmacen) {
    const urlQuery = urlInventario + 'SolicitarAmbientes';
    console.log('ssssssssssssss');
    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
      idAlmacen: _tipoAlmacen
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
    });


    return this.getInfoEviroment().then((env) => {

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

  async ObtenerProductosAlmacenCentral(_idAlmacen) {
    const urlQuery = urlInventario + 'ObtenerProductosAlmacenCentral';

    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
      idAlmacen: _idAlmacen
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
    });


    return this.getInfoEviroment().then((env) => {
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

  async obtenerAlmacenesParaAsignacion(_tipoalmacen) {
    const urlQuery = urlInventario + 'SolicitarAmbientesCompleto';

    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
      idAlmacen: _tipoalmacen
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
    });


    return this.getInfoEviroment().then((env) => {
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


  async productosVendidosPorBarra(_idAlmacen) {
    const urlQuery = urlInventario + 'ProductosVendidosPorBarra';

    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
      idAlmacen: _idAlmacen,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
    });

    console.log('parametros', dataRequest);

    return this.getInfoEviroment().then((env) => {
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

  async ingredientesDeMenuGeneral() {
    const urlQuery = urlInventario + 'IngredientesDeMenuGeneral';

    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
    });

    return this.getInfoEviroment().then((env) => {
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

  async grabarMenuGeneralCompleto(menu: IngredientesDeMenuGeneralDTO) {
    const urlQuery = urlInventario + 'GrabarMenuGeneralCompleto';
    //menu.nombreProducto = '';
    //menu.unidaDeMedida = '';

    await this.getInfoEviroment().then((env) => {
      menu.idSesion = env.session;
      menu.idFechaProceso = env.idFechaProceso;
    });
    console.log('Grabar menu completo',menu);
    return this.getInfoEviroment().then((env) => {
      this.presentLoader();
      return this.httpClient
        .post<any>(urlQuery, JSON.stringify(menu), { headers })
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


  async clasificadorPorTipo(_idClasificadorTipo) {
    const urlQuery = urlInventario + 'ClasificadorPorTipo';

    const dataRequest = {
      idSesion: 0,
      idFechaProceso: _idClasificadorTipo,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
    });

    return this.getInfoEviroment().then((env) => {
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

  async busquedaMenuGeneral(_textoABuscar) {
    const urlQuery = urlInventario + 'BusquedaMenuGeneral';
    let dataRequest = {
      idSesion: 0,
      textoABuscar: _textoABuscar,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
    });

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

  async aperturaInventario() {
    let url_query = urlInventario + 'AperturaInventario';

    let dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso
    });

    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada transaccionesDetallePorID');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async cierreInventario() {

    console.log('SERVICIO CIERRE');

    let url_query = urlInventario + 'CierreInventario';

    let dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso
    });

    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada transaccionesDetallePorID');
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
