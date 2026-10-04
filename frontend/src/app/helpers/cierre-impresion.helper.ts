import { DataDocumento } from 'src/app/interfaces/general/documento';

export interface CierreImpresionRegistro {
  id: string;
  idOperacionDiariaCaja: number;
  cajero: string;
  fechaCierre: string;
  montoTotalCierre: number;
  impreso: boolean;
  documento: DataDocumento;
  pdfBase64?: string | null;
}

const STORAGE_KEY = 'cierresCajeroImpresion';
const MAX_REGISTROS = 10;

/**
 * Historial local de cierres de caja para permitir la reimpresion cuando la
 * impresora falla. El cierre ya quedo registrado en el backend; aqui solo se
 * conserva lo necesario para volver a imprimir el comprobante.
 */
export class CierreImpresionHelper {
  static listar(): CierreImpresionRegistro[] {
    try {
      const raw = localStorage.getItem(STORAGE_KEY);
      if (!raw) {
        return [];
      }
      const parsed = JSON.parse(raw);
      return Array.isArray(parsed) ? parsed : [];
    } catch (error: unknown) {
      console.error('No se pudo leer el historial de cierres:', error);
      return [];
    }
  }

  static obtener(id: string): CierreImpresionRegistro | null {
    return CierreImpresionHelper.listar().find((x) => x.id === id) || null;
  }

  static registrar(registro: CierreImpresionRegistro): void {
    const registros = CierreImpresionHelper.listar().filter((x) => x.id !== registro.id);
    registros.unshift(registro);
    CierreImpresionHelper.persistir(registros.slice(0, MAX_REGISTROS));
  }

  static actualizar(id: string, cambios: Partial<CierreImpresionRegistro>): void {
    const registros = CierreImpresionHelper.listar();
    const indice = registros.findIndex((x) => x.id === id);

    if (indice < 0) {
      return;
    }

    registros[indice] = { ...registros[indice], ...cambios };
    CierreImpresionHelper.persistir(registros);
  }

  static pendientes(): number {
    return CierreImpresionHelper.listar().filter((x) => !x.impreso).length;
  }

  /**
   * Guarda el historial tolerando el limite de cuota del localStorage:
   * si no entra, se descartan los PDF cacheados (el documento se puede
   * regenerar desde el backend al reimprimir).
   */
  private static persistir(registros: CierreImpresionRegistro[]): void {
    try {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(registros));
      return;
    } catch (error: unknown) {
      console.warn('Historial de cierres sin espacio, se descartan PDF cacheados:', error);
    }

    try {
      const sinPdf = registros.map((x, indice) =>
        indice === 0 ? x : { ...x, pdfBase64: null },
      );
      localStorage.setItem(STORAGE_KEY, JSON.stringify(sinPdf));
      return;
    } catch (error: unknown) {
      console.warn('Historial de cierres aun sin espacio, se descarta todo PDF cacheado:', error);
    }

    try {
      const soloDatos = registros.map((x) => ({ ...x, pdfBase64: null }));
      localStorage.setItem(STORAGE_KEY, JSON.stringify(soloDatos));
    } catch (error: unknown) {
      console.error('No se pudo guardar el historial de cierres:', error);
    }
  }
}
