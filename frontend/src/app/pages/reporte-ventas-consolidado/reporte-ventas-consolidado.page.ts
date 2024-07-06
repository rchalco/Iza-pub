import { Component, OnInit } from '@angular/core';
import { FabulaService } from 'src/app/services/fabula.service';

@Component({
  selector: 'app-reporte-ventas-consolidado',
  templateUrl: './reporte-ventas-consolidado.page.html',
  styleUrls: ['./reporte-ventas-consolidado.page.scss'],
})
export class ReporteVentasConsolidadoPage implements OnInit {

  listReporteVentas = [];
  totalCant = 0;
  totalEfect = 0;
  constructor(private fabulaService: FabulaService) { }

  ngOnInit() {
    this.fabulaService.reporteActualVentasProducto().then(service => {
      service.subscribe(resul => {
        console.log(resul);
        if (resul.state !== 1) {
          this.fabulaService.showMessageError(resul.message);
          return;
        }
        this.listReporteVentas = resul.listEntities;
        this.listReporteVentas.forEach(x => {
          this.totalCant += x.cantidad;
          this.totalEfect += x.monto;
        });
      });
    });
  }

}
