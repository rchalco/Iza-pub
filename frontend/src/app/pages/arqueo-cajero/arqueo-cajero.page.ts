import { Component, OnInit } from '@angular/core';
import { VentaService } from 'src/app/services/venta.service';

@Component({
  selector: 'app-arqueo-cajero',
  templateUrl: './arqueo-cajero.page.html',
  styleUrls: ['./arqueo-cajero.page.scss'],
})
export class ArqueoCajeroPage implements OnInit {
  formasPagos = [];
  formasPagosTotales = [];
  montoApertura = 0;
  montoEfectivo = 0;
  ci = '';
  nombreCompleto = '';

  constructor(private ventaService: VentaService) {}

  ngOnInit() {
    this.ventaService.obtenerArqueo().then(async (service) => {
      service.subscribe(async (resul) => {
        this.formasPagos = resul.listEntities;
        console.log('this.formasPagos', this.formasPagos);
        ///TODO llenamos las formas de pagos totales
        this.formasPagos.forEach((x) => {
          this.montoApertura = x.montoApertura;
          if (
            this.formasPagosTotales.filter(
              (yy) => yy.idFormaDePago === x.idFormaDePago
            ).length === 0
          ) {
            const totalFormaPago = {
              idFormaDePago: x.idFormaDePago,
              formaDePago: x.formaDePago,
              total: 0,
            };
            totalFormaPago.total = this.formasPagos
              .filter((zzz) => zzz.idFormaDePago === x.idFormaDePago)
              .reduce((sum, current) => sum + current.montoCubierto, 0);
            this.formasPagosTotales.push(totalFormaPago);
            if (totalFormaPago.idFormaDePago === 1) {
              this.montoEfectivo = this.montoApertura + totalFormaPago.total;
            }
          }
        });
        ///TODO llenamos los totales generales
        await this.ventaService.getInfoEviroment().then((env) => {
          this.ci = env.ci;
          this.nombreCompleto = env.nombreCompleto;
        });
      });
    });
  }
}
