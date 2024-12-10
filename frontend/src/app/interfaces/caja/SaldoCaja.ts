export class SaldoCajaDTO {
  idCaja: number;
  nombreCaja: string = '';
  fechaCierre: string = '';
  saldoInicial: number;
  saldoCierre: number;
  saldoUsuario: number;
  diferencia: number;
  observacion: string = '';
  esCajaActual: boolean;
  estadoCaja: string = '';
  idOperacionDiariaCaja: number;
  idSesion: number;
  fechaApertura: Date;
  observacioApertura: string = '';
  observacionCierre: string = '';
  idFechaProceso: number;
  fechaProceso: Date;
}
