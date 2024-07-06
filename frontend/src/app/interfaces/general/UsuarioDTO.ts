export class UsuarioDTO {
  idUsuario: number;
  usuario_vc: string;
  password: string;
  passwordNuevo: string;
  log_respuesta: string;
  idSesion: number;
  idRol: number;
  idOperacionDiariaCaja: number;
  respuesta: boolean;
  rol_name: string;
  idCaja: number;
  fechaVigenciaHasta: Date;
  idPersona: number;
  activo: boolean;
}
