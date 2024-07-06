import { TransaccionVentasDTO } from 'src/app/interfaces/venta/transaccionVentas';
export class TransaccionVentasDetalleDTO {
    idTransaccionDetalle: number;
    idPedMaster: number;
    idProducto: number;
    nombreProducto: string;
    cantidad: number | null;
    cantidadDisponible: number | null;
    precioVenta: number | null;
    precioUnitario: number | null;
    descuento: number | null;
    observacion: string;
    nroPedido: number | null;
    mesero: string;
    total: number | null;
    transaccion: TransaccionVentasDTO;
}