export interface ResponseCierreGlobalDetalle {
    id: number;
    idOperacionDiariaCaja: number;
    idAlmacen: number;
    descripcion: string;
    usuario: string;
    nombre: string;
    formaDePago: string;
    monto: number;
    cantidad: number;
    total: number;
    montoDeclarado: number;
    diferencia: number;
}