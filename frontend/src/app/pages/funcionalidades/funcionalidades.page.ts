import { Component, OnInit } from '@angular/core';

import {
  FeatureFlagsService,
} from 'src/app/services/feature-flags.service';
import { StockService } from 'src/app/services/stock.service';

@Component({
  standalone: false,
  selector: 'app-funcionalidades',
  templateUrl: './funcionalidades.page.html',
  styleUrls: ['./funcionalidades.page.scss'],
})
export class FuncionalidadesPage implements OnInit {
  /** Estado del toggle. Se sincroniza con el servicio al cargar y al guardar. */
  pagoQr = false;
  cargando = true;
  guardando = false;

  constructor(
    private featureFlags: FeatureFlagsService,
    private stockService: StockService,
  ) {}

  ngOnInit(): void {
    this.featureFlags.cargar().then((flags) => {
      this.pagoQr = flags.pagoQr;
      this.cargando = false;
    });
  }

  /**
   * El toggle ya movio `pagoQr` por el ngModel: si la escritura falla hay que devolverlo
   * al valor real, o la pantalla quedaria mostrando algo que el proximo arranque ignora.
   */
  async cambiarPagoQr(valor: boolean): Promise<void> {
    if (this.guardando || this.cargando || valor === this.featureFlags.pagoQr) {
      return;
    }

    this.guardando = true;

    try {
      await this.featureFlags.guardar({ pagoQr: valor });
      this.pagoQr = valor;
      this.stockService.showMessageSucess(
        valor
          ? 'Cobro por QR habilitado.'
          : 'Cobro por QR deshabilitado. El boton QR cobrara como efectivo.',
      );
    } catch (error) {
      this.pagoQr = this.featureFlags.pagoQr;
      this.stockService.showMessageWarning(
        'No se pudo guardar la configuracion. Intente nuevamente.',
      );
    } finally {
      this.guardando = false;
    }
  }
}
