import { Component, OnInit } from '@angular/core';
import { VentaService } from 'src/app/services/venta.service';

@Component({
  selector: 'app-cierre-operativo',
  templateUrl: './cierre-operativo.page.html',
  styleUrls: ['./cierre-operativo.page.scss'],
})
export class CierreOperativoPage implements OnInit {
  listReporteCierre = [];
  barraCurrent;
  barras: any[] = [];

  constructor(private ventaService: VentaService) {}

  ngOnInit() {
    this.cargarBarras();
  }

  cargarBarras() {
    this.ventaService.obtenerAlmacenes().then((resul) => {
      resul.subscribe((x) => {
        this.barras = x.listEntities;
        console.log('barras', this.barras);
      });
    });
  }

  cargarCierreStock(pIdAlmacen) {
    this.ventaService.obtieneReporteCierre(pIdAlmacen).then((service) => {
      service.subscribe((response) => {
        if (response.state !== 1) {
          this.ventaService.showMessageResponse(response);
        }
        console.log('response', response);
        this.listReporteCierre = response.listEntities;
        this.listReporteCierre.forEach((x) => {
          x.diferencia = 0.0;
          x.stockFinal = x.enStock;
          x.cantidadEntregada = (Math.round(x.enStock * 100) / 100).toFixed(2);
        });
      });
    });
  }
  onChangeEntregado(stock) {
    console.log('stock', stock);
    stock.diferencia = stock.enStock - stock.cantidadEntregada;
    stock.stockFinal = stock.enStock;
  }
  selectBarra(event) {
    this.barraCurrent = event.detail.value;
    console.log('this.barraCurrent', this.barraCurrent);
    this.cargarCierreStock(this.barraCurrent.idAlmacen);
  }
  realizarCierre() {
    this.ventaService
      .realizarCierreStockAlmacen(
        this.barraCurrent.idAlmacen,
        this.listReporteCierre
      )
      .then((services) => {
        services.subscribe((resul) => {
          this.ventaService.showMessageResponse(resul);
        });
      });
  }
}
