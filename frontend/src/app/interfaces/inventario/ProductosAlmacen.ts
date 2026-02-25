export class ProductosAlmancen {

    idClasificador: number;
    clasificador: string;
    idProducto: number;
    nombreProducto: string;
    stockAnterior: number;
    ingresos: number;
    egresos: number;
    ventas: number;
    disponibleAlmacenCentral: number;
    cantidadAsignada: number;
    fechaDeVencimiento: Date = new Date();

}

