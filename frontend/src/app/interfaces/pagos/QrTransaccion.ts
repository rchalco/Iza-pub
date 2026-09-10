/**
 * QR de cobro emitido para una sola venta. Espeja QrTransaccionDTO del backend.
 * Es de un solo uso y de monto fijo: el cobro se verifica por `qrId`, de modo que un
 * pago acreditado sobre ese codigo pertenece a esa venta y a ninguna otra.
 */
export class QrTransaccionDTO {
  qrId: string;
  qrImageBase64: string;
  transactionId: string;
  monto: number;
  moneda: string;
  codigoSucursal: string;
  descripcion: string;
  fechaEmision: string;
  fechaVencimiento: string;
  usoUnico: boolean;
  montoEditable: boolean;
}
