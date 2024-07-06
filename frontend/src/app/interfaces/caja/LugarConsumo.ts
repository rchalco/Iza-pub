export class LugarConsumoDTO {
    idLugarFisico: number;
    idcSala: number | null;
    idcTipoLugar: number | null;
    descripcion: string;
    ocupado: boolean | null;
    cantidadPersonas: number | null;
    salaTipo: string;
    sala: string;
    consumoActual: number;
    picMesa: string;
    idTransaccion: number | null;
    fechaHora: string | null;
    estadoLugar: string;
    colorEstado: string;
}