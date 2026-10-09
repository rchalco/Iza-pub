export class DataDocumento {
  titulo: string = '';
  contenido: string[] = [];
  titulosTabla: string[] = [];
  pie: string = '';
  pathLogo:string = '';
  isTableDocument: boolean = false;
  /** Tabla para impresora termica de 80mm. Ver EngineImpresion.GenerarDocumentoTablaTermica. */
  isThermalTable: boolean = false;
  /** Anchos relativos de las columnas; vacio las reparte en partes iguales. */
  anchosColumnas: number[] = [];
}
