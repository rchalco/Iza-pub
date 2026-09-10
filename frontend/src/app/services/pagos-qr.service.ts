/* eslint-disable @typescript-eslint/naming-convention */
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoadingController, ToastController } from '@ionic/angular';
import { Observable, throwError } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';

import { BaseService } from './baseService';
import { DatabaseService } from './DatabaseService';
import { HEADERS_SERVICE, URL_PAGOSQR } from 'src/environments/environment';

const urlPagosQr = URL_PAGOSQR;
const headers = HEADERS_SERVICE;

/**
 * Cobros por QR de Banco Economico (APIPagosQr). El backend habla con el proxy
 * qr-banco-economico; aqui se emite el QR de una venta, se verifica su cobro y se
 * consulta el reporte del dia.
 *
 * Las consultas de sondeo van con `mostrarLoader = false`: el polling cada 60 s
 * no debe levantar el loader global ni bloquear la caja.
 */
@Injectable({
  providedIn: 'root',
})
export class PagosQrService extends BaseService {
  constructor(
    public databaseService: DatabaseService,
    public httpClient: HttpClient,
    public loadingController: LoadingController,
    public toastController: ToastController,
  ) {
    super(databaseService, httpClient, loadingController, toastController);
  }

  /**
   * Emite el QR de cobro de una venta: un solo uso y monto fijo. Cada llamada genera un
   * codigo nuevo, asi que no debe invocarse dos veces para la misma venta o quedarian dos
   * QR cobrables por el mismo importe.
   */
  generaQrTransaccion(
    monto: number,
    descripcion: string = null,
    codigoSucursal: string = null,
    mostrarLoader = true,
  ): Observable<any> {
    const dataRequest = {
      monto,
      descripcion,
      codigoSucursal,
      moneda: null,
    };

    return this.ejecutar(
      urlPagosQr + 'GeneraQrTransaccion',
      dataRequest,
      'GeneraQrTransaccion',
      mostrarLoader,
    );
  }

  /**
   * Verifica si un QR puntual ya fue cobrado. El backend responde estado 4 (NoData)
   * mientras el pago no se acredite: es la respuesta normal durante la espera.
   */
  consultaPagoQr(
    qrId: string,
    fecha: Date,
    mostrarLoader = false,
  ): Observable<any> {
    const dataRequest = {
      qrId,
      fecha: this.aFechaLocal(fecha),
    };

    return this.ejecutar(
      urlPagosQr + 'ConsultaPagoQr',
      dataRequest,
      'ConsultaPagoQr',
      mostrarLoader,
    );
  }

  /**
   * Cobros por QR acreditados en una fecha. El banco reporta por dia, no por rango:
   * para un periodo hay que iterar dia a dia.
   */
  obtieneQrsPagados(fecha: Date, mostrarLoader = true): Observable<any> {
    const dataRequest = {
      fecha: this.aFechaLocal(fecha),
    };

    return this.ejecutar(
      urlPagosQr + 'ObtieneQrsPagados',
      dataRequest,
      'ObtieneQrsPagados',
      mostrarLoader,
    );
  }

  /**
   * POST comun a los tres endpoints. Sin loader el error tampoco levanta toast:
   * un sondeo que falla se resuelve en el componente, no interrumpe al cajero.
   */
  private ejecutar(
    urlQuery: string,
    dataRequest: any,
    etiqueta: string,
    mostrarLoader: boolean,
  ): Observable<any> {
    if (mostrarLoader) {
      this.presentLoader();
    }

    return this.httpClient
      .post<any>(urlQuery, JSON.stringify(dataRequest), { headers })
      .pipe(
        finalize(() => {
          if (mostrarLoader) {
            this.dismissLoader();
          }
        }),
        catchError((error) => {
          console.error(`Error en ${etiqueta}`, error);
          if (mostrarLoader) {
            this.showMessageError(this.extractErrorMessage(error));
          }
          return throwError(() => new Error(this.extractErrorMessage(error)));
        }),
      );
  }

  /**
   * Fecha en formato local `yyyy-MM-ddT00:00:00`. `toISOString()` la pasaria a UTC y
   * en Bolivia (UTC-4) el reporte terminaria pidiendo el dia anterior.
   */
  private aFechaLocal(fecha: Date): string {
    const anio = fecha.getFullYear();
    const mes = (fecha.getMonth() + 1).toString().padStart(2, '0');
    const dia = fecha.getDate().toString().padStart(2, '0');
    return `${anio}-${mes}-${dia}T00:00:00`;
  }
}
