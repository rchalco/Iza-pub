/* eslint-disable @typescript-eslint/naming-convention */
import { URL_MIROVENTAOPERACION } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './baseService';
import { LoadingController, ToastController } from '@ionic/angular';
import {
  environment,
  HEADERS_SERVICE,
  URL_MIROVENTA,
} from 'src/environments/environment';
import { Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { DatabaseService } from './DatabaseService';
import { async } from '@angular/core/testing';

const urlMicroventa = URL_MIROVENTA;
const headers = HEADERS_SERVICE;

@Injectable({
  providedIn: 'root',
})
export class VentaService extends BaseService {
  constructor(
    public databaseService: DatabaseService,
    public httpClient: HttpClient,
    public loadingController: LoadingController,
    public toastController: ToastController
  ) {
    super(databaseService, httpClient, loadingController, toastController);
  }

  searchProductVenta() {
    const urlQuery = urlMicroventa + 'ObtieneProductosVenta';

    return this.getInfoEviroment().then((env) => {
      const dataRequest = {
        ParametroLong1: environment.idEmpresa,
        ParametroLong2: environment.session,
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

  
  async obtieneProductoInventario(_idAlamcen) {
    const urlQuery = urlMicroventa + 'ObtieneProdcutoInventario';
    let dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
      idAlmacen: _idAlamcen,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest = {
        idSesion: env.session,
        idFechaProceso: env.idFechaProceso,
        idAlmacen: _idAlamcen,
      };
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

  async obtieneProductoAlmacen(_idAlamcen) {
    const urlQuery = urlMicroventa + 'ObtienePorAlmacen';
    let dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
      idAlmacen: _idAlamcen,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest = {
        idSesion: environment.session,
        idFechaProceso: environment.idFechaProceso,
        idAlmacen: _idAlamcen,
      };
    });

    console.log('XXXXXXdatos enviados para buscar productos', dataRequest);

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
  registrarVenta(pDetalleVentas, pListaFormaDePago, _idAlmacen, _observacion) {
    const urlQuery = urlMicroventa + 'RegistrarVentas';
    console.log('_idAlmacen', _idAlmacen);

    return this.getInfoEviroment().then((env) => {
      const dataRequest = {
        idSesion: env.session,
        idOperacionDiariaCaja: env.idOperacionDiariaCaja,
        detalleVentas: pDetalleVentas,
        idAlmancen: _idAlmacen,
        idFechaProceso: env.idFechaProceso,
        formasDePago: pListaFormaDePago,
        Observaciones: _observacion,
      };
      this.presentLoader();
      console.log('registrarVenta req', dataRequest);
      return this.httpClient
        .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
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

  

  async obtenerFormasDePago() {
    const urlQuery = urlMicroventa + 'ObtieneFormasdePago';
    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
    });
    this.presentLoader();

    return this.httpClient
      .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada obtenerFormasDePago');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async obtenerArqueo() {
    const urlQuery = urlMicroventa + 'UltimasMovimientos';
    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
      idAlmacen: 0,
      idEstado: 0,
      idOperacionDiaria: 0,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
      dataRequest.idAlmacen = env.idAlmacen;
      dataRequest.idOperacionDiaria = env.idOperacionDiariaCaja;
    });

    this.presentLoader();

    return this.httpClient
      .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada obtenerFormasDePago');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async obtenerArqueoCajero(_idOperacion) {
    const urlQuery = urlMicroventa + 'UltimasMovimientos';
    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
      idAlmacen: 0,
      idEstado: 0,
      idOperacionDiaria: _idOperacion,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
    });

    console.log('obtenerArqueoCajero req', dataRequest);
    this.presentLoader();

    return this.httpClient
      .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada obtenerFormasDePago');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async obtenerCajeros() {
    const urlQuery = urlMicroventa + 'ObtieneCajerosCierre';
    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
    });

    this.presentLoader();

    return this.httpClient
      .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada obtenerFormasDePago');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async obtieneReporteCierre(pIdAlmacen) {
    const urlQuery = urlMicroventa + 'ObtieneReporteCierre';
    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
      idAlmacen: pIdAlmacen,
      idEstado: 0,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
      dataRequest.idAlmacen = pIdAlmacen;
    });

    this.presentLoader();

    return this.httpClient
      .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada obtieneReporteCierre');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async obtieneReporteCierreTotal() {
    const urlQuery = urlMicroventa + 'ReporteCierreGlobal';
    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
    });

    this.presentLoader();

    return this.httpClient
      .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada obtieneReporteCierreTotal');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async obtieneReporteCierreGlobalCajero() {
    const urlQuery = urlMicroventa + 'ReporteCierreGlobalDetalle';
    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
    });

    this.presentLoader();

    return this.httpClient
      .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada obtieneReporteCierreTotal');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async reporteCierreDominium() {
    const urlQuery = urlMicroventa + 'ReporteCierreDominium';
    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
    });

    this.presentLoader();

    return this.httpClient
      .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada obtieneReporteCierreTotal');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async realizarCierreStockAlmacen(pIdAlmacen, pDetalle) {
    const urlQuery = urlMicroventa + 'CerrarStockAlmacen';
    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
      idAlmacen: pIdAlmacen,
      idEstado: 0,
      detalle: pDetalle,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
      dataRequest.idAlmacen = pIdAlmacen;
    });

    this.presentLoader();

    return this.httpClient
      .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada realizarCierreStockAlmacen');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async realizarCierreCaja(
    pDetalle,
    pMontoApertura,
    pMontoTotalCierre,
    pIdOperacionDiariaCaja
  ) {
    const urlQuery = urlMicroventa + 'CerrarCaja';
    const dataRequest = {
      idSesion: 0,
      idfechaproceso: 0,
      idOperacionDiariaCaja: pIdOperacionDiariaCaja,
      detalle: pDetalle,
      montoApertura: pMontoApertura,
      montoTotalCierre: pMontoTotalCierre,
      idEstado: 0,
      Observaciones: '',
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idfechaproceso = env.idFechaProceso;
    });

    this.presentLoader();

    return this.httpClient
      .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada realizarCierreStockAlmacen');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async obtieneCajeroDeUnaFecha() {
    const urlQuery = urlMicroventa + 'ObtieneCajeroDeUnaFecha';
    const dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idFechaProceso;
    });

    this.presentLoader();

    return this.httpClient
      .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada obtenerFormasDePago');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async grabarBufferInventario(inventarios) {
    const urlQuery = urlMicroventa + 'GrabarBufferInventario';
    const dataRequest = {
      inventarioFinalDTO: inventarios
    };
    // await this.getInfoEviroment().then((env) => {
    //   dataRequest.idSesion = env.session;
    //   dataRequest.idFechaProceso = env.idFechaProceso;
    // });

    //this.presentLoader();

    return this.httpClient
      .post<any>(urlQuery, JSON.stringify(inventarios), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada obtenerFormasDePago');
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
