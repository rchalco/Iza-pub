import { TransaccionVentasDetalleDTO } from "./transaccionVentasDetalle";

export class TransaccionVentasDTO {
    idPedMaster: number;
    idCajaOperacionDiariaCaja: number;
    idcRelacion: number | null;
    idcTipoTransaccion: number | null;
    nombreRelacion: string;
    nombre: string;
    dI: string;
    fechaHora: string | null;
    montoEntrada: number | null;
    montoSalida: number | null;
    descuento: number | null;
    idcEstado: number | null;
    estado: string;
    fechaCambioEstado: string | null;
    idAmbiente: number;
    observaciones: string;
    esEfectivo: boolean | null;
    comision: number | null;
    bancoDestinoComision: string;
    idcFormaPagoComision: number | null;
    idcFormaPago: number | null;
    session: number
    transaccionDetalle: TransaccionVentasDetalleDTO[];
    idSesion: number;
    idEmpresa: number;

}