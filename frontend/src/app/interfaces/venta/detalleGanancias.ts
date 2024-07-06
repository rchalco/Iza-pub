export class DetalleGananciasDTO {
    anio: number | null;
    mes: number | null;
    fecha: string | null;
    fechaReal: string | null;
    idTransaccion: number | null;
    nombre: string;
    asignadoA: string;
    descuento: number | null;
    cantidad: number | null;
    precioVenta: number | null;
    totalVenta: number | null;
    precioUnitario: number | null;
    totalReal: number | null;
    totalGanancia: number | null;
    tipoProducto: string;
    producto: string;
    personaAtiende: string;
}