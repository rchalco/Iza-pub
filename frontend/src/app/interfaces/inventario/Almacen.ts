export class AlmacenDTO {
    idAlmacen: number;
    idSesion: number | null;
    idUsuario: number;
    idTipoAlmacen: number | null;
    descripcion: string;
    fechaRegistro: string;
    fechaVigenciaHasta: string | null;
}