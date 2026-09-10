/** Cobro acreditado sobre un QR de Iza. Espeja QrPagadoDTO del backend. */
export class QrPagadoDTO {
  qrId: string;
  transactionId: string;
  fechaPago: string;
  horaPago: string;
  moneda: string;
  monto: number;
  codigoBancoOrigen: string;
  nombrePagador: string;
  documentoPagador: string;
  cuentaPagador: string;
  descripcion: string;
  codigoSucursal: string;
}
