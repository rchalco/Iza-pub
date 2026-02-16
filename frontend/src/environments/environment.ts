/* eslint-disable @typescript-eslint/naming-convention */
// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

import { HttpHeaders } from '@angular/common/http';

///Configuraciones:
///Lavanderia Jeffry: idEmpresa: 1 BD: DBTintoreriaGamaFac
///Snack Perfecto: idEmpresa: 1 BD: GamaFac
export const verionsApp = '2.2';
export const environment = {
  production: false,
  idEmpresa: 1,
  idUsuario: 2,
  idOperacionDiariaCaja: 0,
  Usuario: 'ADMIN',
  UsuarioLabel: 'ADMIN',
  session: 0,
  idCaja: 0,
  idRol: 0,
  idAlmacen: 0,
  rol: '',
  ci: '',
  nombreCompleto: '',
  idFechaProceso: 0,
  fechaProceso: new Date(),
};
export const HEADERS_SERVICE = new HttpHeaders({
  'Access-Control-Allow-Origin': '*',
  'Access-Control-Allow-Methods': 'POST, GET, OPTIONS, PUT',
  Accept: '*/*',
  'content-type': 'application/json',
});

// Create our number formatter.
export const customFormatter = (data) => {
  const format = new Intl.NumberFormat('es-BO', {
    style: 'currency',
    currency: 'USD',
    // These options are needed to round to whole numbers if that's what you want.
    //minimumFractionDigits: 0, // (this suffices for whole numbers, but will print 2500.10 as $2,500.1)
    //maximumFractionDigits: 0, // (causes 2500.99 to be printed as $2,501)
  });
  return format.format(data).replace('USD', 'Bs.');
};

//DEV
// export const URL_MIROVENTA = 'http://localhost:8001/api/Microventa/';
// export const URL_MIROVENTAOPERACION =
//   'http://localhost:8001/api/MicroventaOperacion/';
// export const URL_TINTORERIA = 'http://localhost:8001/api/Tintoreria/';
// export const URL_SECURITY = 'http://localhost:8001/api/Seguridad/';
// export const URL_PERSON = 'http://localhost:8001/api/Person/';

export const LogoVoucher = '';
//PROD
// export const URL_MIROVENTA = 'http://181.188.169.10:8001/api/Microventa/';
// export const URL_MIROVENTAOPERACION = 'http://181.188.169.10:8001/api/MicroventaOperacion/';

//SERVER OFICIAL
export const URL_MIROVENTAOPERACION =
  'http://localhost:8001/api/MicroventaOperacion/';
export const URL_TINTORERIA = 'http://localhost:8001/api/Tintoreria/';
//El de seguridad no debe cambiar el puerto
//export const URL_SECURITY = 'http://155.138.212.216:8033/api/APISecurity/';
export const URL_SECURITY = 'http://155.138.212.216:8034/api/APISeguridad/';
export const URL_INVENTARIO = 'http://155.138.212.216:8034/api/APIIventario/';
export const URL_MIROVENTA = 'http://155.138.212.216:8034/api/APIVenta/';
export const URL_PERSON = 'http://localhost:8001/api/Person/';
export const URL_CARDS = 'http://localhost:8001/api/Tarjeta/';
export const URL_FINGERS = 'http://localhost:8001/api/Biometric/';

//SERVER PRUEBA

// export const URL_MIROVENTAOPERACION = 'http://localhost:8001/api/MicroventaOperacion/';
// export const URL_TINTORERIA = 'http://localhost:8001/api/Tintoreria/';
// export const URL_SECURITY = 'http://localhost:5294/api/APISeguridad/';
// export const URL_INVENTARIO = 'http://localhost:5294/api/APIIventario/';
// export const URL_MIROVENTA = 'http://localhost:5294/api/APIVenta/';
// //export const URL_SECURITY = 'http://localhost:5294/api/APISeguridad/';
// export const URL_PERSON = 'http://localhost:8001/api/Person/';
// export const URL_CARDS = 'http://localhost:8001/api/Tarjeta/';
// export const URL_FINGERS = 'http://localhost:8001/api/Biometric/';
// /*
//  * For easier debugging in development mode, you can import the following file
//  * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
//  *
//  * This import should be commented out in production mode because it will have a negative impact
//  * on performance if an error is thrown.
//  */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
