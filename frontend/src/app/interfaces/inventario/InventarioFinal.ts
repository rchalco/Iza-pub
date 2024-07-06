export class InventarioFinalDTO {
  id: number;
  idProducto: number;
  idClasificador: number;
  categoria: string;
  nombreProducto: string;
  costoUnitario: number;
  almacenCentral: number;
  almacenOtroProveedor: number;
  saldoTotal: number;
  ingreso: number;
  totalIngresos: number;
  asignacionesBarra1: number;
  totalAsignacionesBarra1: number;
  asignacionesBarra2: number;
  totalAsignacionesBarra2: number;
  asignacionesBarra3: number;
  totalAsignacionesBarra3: number;
  devolucionesB1: number;
  devolucionesB2: number;
  devolucionesB3: number;
  totalDevoluciones: number;
  precioVenta: number;
  valorGanado: number;
}
