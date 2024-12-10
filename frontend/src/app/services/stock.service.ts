/* eslint-disable prefer-const */
/* eslint-disable @typescript-eslint/naming-convention */
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './baseService';
import { LoadingController, ToastController } from '@ionic/angular';
import {
  environment,
  HEADERS_SERVICE,
  URL_MIROVENTA,
  URL_MIROVENTAOPERACION,
  URL_SECURITY,
} from 'src/environments/environment';
import { Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { DatabaseService } from './DatabaseService';

const urlMicroventa = URL_MIROVENTA;
const urlMicroventaOperacion = URL_MIROVENTAOPERACION;
const urlSeguridad = URL_SECURITY;
const headers = HEADERS_SERVICE;

@Injectable({
  providedIn: 'root',
})
export class StockService extends BaseService {
  constructor(
    public databaseService: DatabaseService,
    public httpClient: HttpClient,
    public loadingController: LoadingController,
    public toastController: ToastController
  ) {
    super(databaseService, httpClient, loadingController, toastController);
  }

  async searchProduct() {
    let url_query = urlMicroventa + 'SearchProduct';

    let dataRequest = {
      idEmpresa: environment.idEmpresa,
    };

    console.log('datos enviados para buscar productos', dataRequest);

    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
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

  registrarCompra(
    _idProducto,
    _cantidad,
    _precioUnitario,
    _unidad,
    _cantidadXcaja,
    _precioCaja
  ) {
    let url_query = urlMicroventa + 'RegistrarCompra';

    let dataRequest = {
      idSession: environment.session,
      idOperacionDiariaCaja: environment.idOperacionDiariaCaja,
      idProducto: _idProducto,
      cantidad: _cantidad,
      precioUnitario: _precioUnitario,
      unidadXCaja: _cantidadXcaja,
      precioCaja: _precioCaja,
      tipoUnidad: _unidad,
    };

    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada registrarCompra');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error('dataRequest', dataRequest);
          console.error('error', error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  ///FIN SEGURIDAD

  async ultimasCajas(_estadoCaja) {
    let url_query = urlMicroventa + 'UltimasCajas';

    let dataRequest = {
      idSesion: 0,
      EstadoCaja: _estadoCaja,
      idCaja: environment.idCaja,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idCaja = env.idCaja;
      console.log('*CAJA INGRESADA', env.idCaja);
    });

    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada ultimasCajas');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async cierreCaja(_idCaja, _montoCierre, _observacion) {
    let url_query = urlMicroventa + 'CierreCaja';

    let dataRequest = {
      idCaja: _idCaja,
      SaldoUsuario: _montoCierre,
      Observacion: _observacion,
      idOperacionDiariaCaja: environment.idOperacionDiariaCaja,
      idSesion: environment.session,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
    });

    console.log('**se termino la llamada CierreCaja', _idCaja);
    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada CierreCaja');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async obtieneCaja(_fechaSeleccionada) {
    let url_query = urlMicroventa + 'ObtieneCaja';

    let dataRequest = {
      ParametroLong2: environment.idEmpresa,
      ParametroLong3: environment.session,
      ParametroFecha1: _fechaSeleccionada,
      ParametroLong1: environment.idCaja,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.ParametroLong1 = env.idCaja;
      dataRequest.ParametroLong2 = environment.idEmpresa;
      dataRequest.ParametroLong3 = environment.session;
    });

    //console.log('IDCAJA',environment.idCaja);
    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada obtieneCaja');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async aperturaCaja(
    _fechaSeleccionada,
    _saldoInicial,
    _observacion,
    _idAlmacen
  ) {
    let url_query = urlMicroventa + 'AperturaCaja';

    let dataRequest = {
      FechaApertura: _fechaSeleccionada,
      SaldoInicial: _saldoInicial,
      Observacion: _observacion,
      idCaja: environment.idCaja,
      idSesion: environment.session,
      //idEmpresa: environment.idEmpresa,
      idAlmacen: _idAlmacen,
      nombreCaja: '',
      FechaCierre: '',
      SaldoCierre: 0,
      SaldoUsuario: 0,
      diferencia: 0,
      EsCajaActual: false,
      EstadoCaja: '',
      idOperacionDiariaCaja: 0

    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.idCaja = env.idCaja;
      dataRequest.idSesion = env.session;
      //dataRequest.idEmpresa = env.idEmpresa;
    });
    console.log('APERTURA CAJA', dataRequest);
    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada obtieneCaja');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async lugarConsumo() {
    let url_query = urlMicroventa + 'LugarConsumo';

    let dataRequest = {
      ParametroLong1: environment.idEmpresa,
      ParametroLong2: environment.session,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.ParametroLong1 = env.idEmpresa;
      dataRequest.ParametroLong2 = env.session;
    });

    //console.log('IDCAJA',environment.idCaja);
    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada lugarConsumo');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async listaMeseros() {
    let url_query = urlMicroventa + 'ListaMeseros';

    let dataRequest = {
      ParametroLong1: environment.idEmpresa,
      ParametroLong2: environment.session,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.ParametroLong1 = env.idEmpresa;
      dataRequest.ParametroLong2 = env.session;
    });

    //console.log('IDCAJA',environment.idCaja);
    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada listaMeseros');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async grabaPedido1(_transaccionPedido) {
    let url_query = urlMicroventa + 'GrabaPedido1';

    let dataRequest = {
      ParametroLong1: environment.idEmpresa,
      ParametroLong2: environment.session,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.ParametroLong1 = env.idEmpresa;
      dataRequest.ParametroLong2 = env.session;
    });

    //console.log('IDCAJA',environment.idCaja);
    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada graba pedido');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async grabaPedido(_transaccionPedido) {
    let url_query = urlMicroventa + 'GrabaPedido';

    let dataRequest = {
      idAmbiente: _transaccionPedido.idAmbiente,
      idCaja: 0,
      idSesion: 0,
      idCajaOperacionDiariaCaja: _transaccionPedido.idCajaOperacionDiariaCaja,
      idcTipoTransaccion: _transaccionPedido.idcTipoTransaccion,
      fechaHora: _transaccionPedido.fechaHora,
      idPedMaster: _transaccionPedido.idPedMaster,
      observaciones: '',
      transaccionDetalle: _transaccionPedido.transaccionDetalle,

      //transaccionVentas: _transaccionPedido,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.idCaja = env.idCaja;
      dataRequest.idSesion = env.session;
    });

    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada grabaPedido');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async finalizarPedido(_transaccionPedido) {
    let url_query = urlMicroventa + 'FinalizarPedido';

    let dataRequest = {
      transaccionVentas: _transaccionPedido,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.transaccionVentas.idCaja = env.idCaja;
      dataRequest.transaccionVentas.idSesion = env.session;
    });

    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada grabaPedido');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async transaccionesDetallePorID(_idTransaccion) {
    let url_query = urlMicroventa + 'TransaccionesDetallePorID';

    let dataRequest = {
      ParametroLong1: _idTransaccion,
      ParametroLong2: 0,
      ParametroLong3: 0,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.ParametroLong2 = env.idCaja;
      dataRequest.ParametroLong3 = env.session;
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

  async cambioDeMesa(_idLugarFisicoAntiguo, _idLugarFisicoNuevo) {
    let url_query = urlMicroventa + 'CambioDeMesa';

    let dataRequest = {
      ParametroLong1: _idLugarFisicoAntiguo,
      ParametroLong2: _idLugarFisicoNuevo,
      ParametroLong3: 0,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.ParametroLong3 = env.session;
    });

    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada cambioDeMesa');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async clasificadorPorTipo(_idClasificadorTipo) {
    let url_query = urlMicroventa + 'ClasificadorPorTipo';

    let dataRequest = {
      ParametroLong1: _idClasificadorTipo,
    };

    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada clasificadorPorTipo');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async graficoVentaPorProducto(_fechaInicial, _fechaFinal) {
    let url_query = urlMicroventa + 'GraficoVentaPorProducto';

    let dataRequest = {
      ParametroLong1: environment.idEmpresa,
      ParametroLong2: environment.idFechaProceso,
      ParametroFecha1: _fechaInicial,
      ParametroFecha2: _fechaFinal,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.ParametroLong1 = env.session;
      dataRequest.ParametroLong2 = env.idFechaProceso;
    });

    console.error('graficooooooo');

    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada graficoVentaPorProducto');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

 
  async grabaAsignacionProducto(
    idAlmacenOrigen,
    idAlmacenDestino,
    _detalleProducto
  ) {
    let url_query = urlMicroventa + 'GrabaAsignacionProducto';

    let dataRequest = {
      idSesion: 0,
      idfechaproceso: 0,
      idAlmacenDesde: idAlmacenOrigen,
      idAlmacenHasta: idAlmacenDestino,
      observaciones: '',
      detalleProductos: _detalleProducto,

      //transaccionVentas: _transaccionPedido,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.idfechaproceso = env.idFechaProceso;
      dataRequest.idSesion = env.session;
    });

    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada GrabaAsignacionProducto');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async obtieneProductosCentral() {
    let url_query = urlMicroventa + 'ObtieneProductosCentral';

    let dataRequest = {
      ParametroLong1: 0,
      ParametroLong2: 0,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.ParametroLong1 = env.session;
      dataRequest.ParametroLong2 = env.idFechaProceso;
    });

    console.log('ObtieneProductosCentral');

    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada obtiene almacen central');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }
  async grabaAsignacionCompra(_detalleCompra) {
    let url_query = urlMicroventa + 'GrabaAsignacionCompra';

    let dataRequest = {
      idSesion: 0,
      idfechaproceso: 0,
      observaciones: '',
      detalleProductos: _detalleCompra,
      //transaccionVentas: _transaccionPedido,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.idfechaproceso = env.idFechaProceso;
      dataRequest.idSesion = env.session;
    });

    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada GrabaAsignacionProducto');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async obtieneStockPedidosFecha() {
    let url_query = urlMicroventa + 'ObtieneStockPedidosFecha';

    let dataRequest = {
      idSesion: 0,
      idfechaproceso: 0,
      idAlmacen: 0,
    };
    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idfechaproceso = env.idFechaProceso;
      dataRequest.idAlmacen = env.idAlmacen;
    });

    console.log('ObtieneStockPedidosFecha, Request: ', dataRequest);

    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada obtieneStockPedidosFecha');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async grabaEntradaSalida(_monto, _motivo, _entradaSalida) {
    let url_query = urlMicroventa + 'GrabaEntradaSalida';

    let dataRequest = {
      ParametroLong1: 0,
      ParametroLong2: 0,
      ParametroLong3: 0,
      ParametroTexto1: _motivo,
      ParametroDecimal1: _monto,
      ParametroInt1: _entradaSalida,
      //transaccionVentas: _transaccionPedido,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.ParametroLong2 = env.idFechaProceso;
      dataRequest.ParametroLong1 = env.session;
      dataRequest.ParametroLong3 = env.idOperacionDiariaCaja;
    });

    console.log('datos entrada', dataRequest);
    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada GrabaEntradaSalida');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async obtenerPersonas() {
    let url_query = urlMicroventa + 'ObtenerPersonas';

    let dataRequest = {
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
          console.log('**se termino la llamada graficoVentaPorProducto');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async grabarPersona(_persona) {
    let url_query = urlMicroventa + 'GrabarPersona';

    // console.log('persona 2',_persona);
    let dataRequest = {
      idPersona: _persona.idPersona,
      DocumentoDeIdentidad: _persona.documentoDeIdentidad,
      ApellidoPaterno: _persona.apellidoPaterno,
      ApellidoMaterno: _persona.apellidoMaterno,
      Nombres: _persona.nombres,
      celular: _persona.celular,
    };

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

  async obtenerUsuarios() {
    let url_query = urlMicroventa + 'ObtenerUsuarios';

    let dataRequest = {
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
          console.log('**se termino la llamada graficoVentaPorProducto');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async obtenerRoles() {
    let url_query = urlMicroventa + 'ObtenerRol';

    let dataRequest = {
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
          console.log('**se termino la llamada graficoVentaPorProducto');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async grabarUsuario(_usuario) {
    let url_query = urlMicroventa + 'GrabarUsuario';

    // console.log('persona 2',_persona);
    let dataRequest = {
      IdRol: _usuario.idRol,
      IdPersona: _usuario.idPersona,
      usuario_vc: _usuario.usuario_vc,
      FechaVigenciaHasta: _usuario.fechaVigenciaHasta,
      Password: _usuario.password,
      PasswordNuevo: _usuario.passwordNuevo,
      idUsuario: _usuario.idUsuario,
      idSesion: 0,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
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

  async obtieneListaCajeroCompleto() {
    let url_query = urlMicroventa + 'ObtieneListaCajeroCompleto';

    let dataRequest = {
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
          console.log('**se termino la llamada ObtieneListaCajeroCompleto');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async actualizaCajerosActivos(
    _detalleCajeros
  ) {
    let url_query = urlMicroventa + 'ActualizaCajerosActivos';

    let dataRequest = {
      idSesion: 0,
      detalleCajeros: _detalleCajeros,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
    });

    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada actualizaCajerosActivos');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async grabarBufferInventario(requestBuffer) {
    let url_query = urlMicroventaOperacion + 'GrabarBufferInventario';

    let dataRequest = {
      idSesion: 0,
      idFechaProceso: 0,
      detalle: requestBuffer,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.idSesion = env.session;
      dataRequest.idFechaProceso = env.idfechaproceso;
    });

    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada grabarBufferInventario');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  async ObtieneMovimientoAlmacenCentral() {
    let url_query = urlMicroventa + 'ObtieneMovimientoAlmacenCentral';

    let dataRequest = {
      ParametroLong1: 0,
      ParametroLong2: 0,
    };

    await this.getInfoEviroment().then((env) => {
      dataRequest.ParametroLong1 = env.session;
      dataRequest.ParametroLong2 = env.idfechaproceso;
    });

    this.presentLoader();
    return this.httpClient
      .post<any>(url_query, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada ObtieneListaCajeroCompleto');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      );
  }

  grabaReposicionProductosJSON(_changes: any) {
    let url_query = urlMicroventa + 'GrabaReposicionProductosJSON';

    let dataRequest = {
      ParametroLong1: 0,
      ParametroLong2: 0,
      ParametroTexto1: _changes
    };

    this.getInfoEviroment().then((env) => {
      dataRequest.ParametroLong1 = env.session;
      dataRequest.ParametroLong2 = env.idfechaproceso;
    });

    this.presentLoader();
    return this.httpClient
      .post(url_query, JSON.stringify(dataRequest), {
        responseType: 'arraybuffer',
        headers: headers,
      })
      .pipe(
        finalize(() => {
          console.log('**se termino la llamada ObtieneListaCajeroCompleto');
          this.dismissLoader();
        }),
        catchError((error) => {
          console.error(error);
          this.showMessageError('No se tiene comunicacion con el servidor');
          return Observable.throw(new Error(error.status));
        })
      ).subscribe(resul => {
        console.log('resultado reporte', resul);
        this.downLoadFile(resul, "application/pdf")
      })

      ;
  }

  downLoadFile(data: any, _type: string) {
    const blob = new Blob([data], { type: _type });
    const url = window.URL.createObjectURL(blob);
    const pwa = window.open(url);
    if (!pwa || pwa.closed || typeof pwa.closed == 'undefined') {
      alert(
        'Por favor deshabilite los bloqueadores de descarga para continuar.'
      );
    }
  }

  

}
