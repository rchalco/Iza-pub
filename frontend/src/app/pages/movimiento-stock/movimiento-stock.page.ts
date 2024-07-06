/* eslint-disable @typescript-eslint/member-ordering */
import { Component, OnInit } from '@angular/core';
import { StockService } from 'src/app/services/stock.service';

@Component({
  selector: 'app-movimiento-stock',
  templateUrl: './movimiento-stock.page.html',
  styleUrls: ['./movimiento-stock.page.scss'],
})
export class MovimientoStockPage implements OnInit {
  constructor(private stockService: StockService) {}
  dataMovimiento;
  ngOnInit() {
    this.cargarMovimientos();
  }
  cargarMovimientos() {
    this.stockService.obtieneStockPedidosFecha().then((services) => {
      services.subscribe((resulService) => {
        if (resulService.state !== 1) {
          this.stockService.showMessageResponse(resulService);
        }
        this.dataMovimiento = resulService.object;
      });
    });
  }
}
