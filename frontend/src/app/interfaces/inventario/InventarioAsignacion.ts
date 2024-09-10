import { InventarioProducto } from "./InventarioProducto";
export class InventarioAsignacion {
    idSesion: number;
    idfechaproceso: number;
    idAlmacenDesde: number;
    idAlmacenHasta: number;
    observaciones: string;
    origen: string;
    destino: string;
    idOperacionDiariaCaja: number;
    detalleProductos: InventarioProducto[];
}