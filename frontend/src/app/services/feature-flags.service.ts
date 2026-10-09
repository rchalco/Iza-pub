import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

import { DatabaseService } from './DatabaseService';
import { FEATURE_PAGO_QR } from 'src/environments/environment';

/** Clave del almacenamiento local donde vive la configuracion de funcionalidades. */
const CLAVE_FLAGS = 'featureFlags';

/** Funcionalidades que se pueden apagar desde la pantalla de configuracion. */
export interface FeatureFlags {
  /**
   * Interfaz de cobro por QR. En false el boton QR de venta express cobra como el
   * efectivo. El boton nunca se bloquea.
   */
  pagoQr: boolean;
}

/**
 * Funcionalidades activables desde la aplicacion, persistidas en el equipo.
 *
 * El valor vive en memoria ademas del almacenamiento porque quien decide el flujo de
 * cobro (`realizarPago`) es sincrono: no puede esperar una lectura de disco en medio de
 * una venta. El valor de `environment` queda como estado inicial de un equipo nuevo.
 */
@Injectable({
  providedIn: 'root',
})
export class FeatureFlagsService {
  private flags: FeatureFlags = { pagoQr: FEATURE_PAGO_QR };
  private readonly cambios = new BehaviorSubject<FeatureFlags>({ ...this.flags });
  private cargaEnCurso: Promise<FeatureFlags> = null;

  constructor(private databaseService: DatabaseService) {}

  /** Emite en cada cambio para que las pantallas reaccionen sin recargar la aplicacion. */
  get cambios$(): Observable<FeatureFlags> {
    return this.cambios.asObservable();
  }

  /** Lectura sincrona para plantillas y flujos que no pueden esperar. */
  get pagoQr(): boolean {
    return this.flags.pagoQr;
  }

  get valores(): FeatureFlags {
    return { ...this.flags };
  }

  /**
   * Trae la configuracion guardada al cache en memoria. Se llama al arrancar la app; si
   * el equipo nunca guardo nada, quedan los valores de `environment`.
   */
  cargar(): Promise<FeatureFlags> {
    // Varias pantallas la llaman al iniciar: se comparte la misma lectura en vuelo.
    if (this.cargaEnCurso) {
      return this.cargaEnCurso;
    }

    this.cargaEnCurso = this.databaseService
      .getItem(CLAVE_FLAGS)
      .then((guardado) => {
        if (guardado) {
          this.aplicar(guardado);
        }
        return this.valores;
      })
      .catch((error) => {
        // Un almacenamiento ilegible no puede dejar la caja sin formas de pago:
        // se sigue con los valores de compilacion.
        console.error('No se pudo leer la configuracion de funcionalidades', error);
        return this.valores;
      })
      .finally(() => {
        this.cargaEnCurso = null;
      });

    return this.cargaEnCurso;
  }

  /** Guarda un cambio y avisa a quien dependa de el. */
  async guardar(cambios: Partial<FeatureFlags>): Promise<FeatureFlags> {
    const anterior = this.valores;
    this.aplicar(cambios);

    try {
      await this.databaseService.setItem(CLAVE_FLAGS, this.valores);
    } catch (error) {
      // Si no se pudo persistir se revierte: la pantalla mostraria un estado que el
      // proximo arranque no respeta.
      this.flags = anterior;
      this.cambios.next(this.valores);
      throw error;
    }

    return this.valores;
  }

  private aplicar(cambios: Partial<FeatureFlags>): void {
    this.flags = {
      pagoQr:
        typeof cambios?.pagoQr === 'boolean' ? cambios.pagoQr : this.flags.pagoQr,
    };
    this.cambios.next(this.valores);
  }
}
