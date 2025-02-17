export class InventarioProducto {
    idProducto: number;
    nombreProducto: string;
    categoria: string;
    cantidad: number;
    cantidadCaja: number;
    enStock: number;
    idProveedor: number;
    montoCompra: number;
    idTipo: number;
    montoTotal: number;
    fechaDeVencimiento: Date = new Date();
    idFechaProceso: number;
    fechaProceso: Date = new Date();
}