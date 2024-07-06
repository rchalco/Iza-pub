import {
  AfterViewInit,
  Component,
  OnInit,
  ElementRef,
  ViewChild,
} from '@angular/core';
//import { Chart } from 'chart.js';
import Chart from 'chart.js/auto';
import { DetalleGananciasDTO } from 'src/app/interfaces/venta/detalleGanancias';
import { StockService } from 'src/app/services/stock.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-reporte-tablero',
  templateUrl: './reporte-tablero.page.html',
  styleUrls: ['./reporte-tablero.page.scss'],
})
export class ReporteTableroPage implements OnInit {
  @ViewChild('lineChartCanvas') private charLineRef: ElementRef;
  @ViewChild('doughnutCanvas') private doughnutRef: ElementRef;
  @ViewChild('barCanvas') private barCharRef: ElementRef;
  @ViewChild('barCanvasMonto') private barMontoCharRef: ElementRef;

  showTablero = false;
  fechaSeleccionadaIni: any = new Date().toISOString();
  fechaSeleccionadaFin: any = new Date().toISOString();

  ///TABLERO ESTILO DONUTS
  chart: any;
  datosTablero: any = [];
  labelsTablero: any = [];
  backgoundColorsTablero: any = [
    'rgba(255, 159, 64, 0.8)',
    'rgba(255, 99, 132, 0.8)',
    'rgba(54, 162, 235, 0.8)',
    'rgba(255, 206, 86, 0.8)',
    'rgba(75, 192, 192, 0.8)',
    'rgba(229, 0, 255, 0.8)',
    'rgba(0, 255, 127, 0.8)',
    'rgba(255, 233, 0, 0.8)',
    'rgba(0, 182, 255, 0.8)',
  ];
  ///TABLERO ESTILO BAR
  barChart: any;
  datosTableroBar: any = [];
  borderColorBar: any = [
    'rgba(255,99,132,1)',
    'rgba(54, 162, 235, 1)',
    'rgba(255, 206, 86, 1)',
    'rgba(75, 192, 192, 1)',
    'rgba(153, 102, 255, 1)',
    'rgba(255, 159, 64, 1)'
  ];

  backgroundBar: any = [
    'rgba(255, 99, 132, 0.2)',
    'rgba(54, 162, 235, 0.2)',
    'rgba(255, 206, 86, 0.2)',
    'rgba(75, 192, 192, 0.2)',
    'rgba(153, 102, 255, 0.2)',
    'rgba(255, 159, 64, 0.2)'
  ];
  barCanvasMonto: any;
  datosTableroMontoBar: any = [];
  ///TABLERO ESTILO LINE
  lineChart: any;
  listaVentasPorProducto: DetalleGananciasDTO[] = [];


  constructor(private stockService: StockService) { }
  ngOnInit() {

    this.showTablero = false;
    this.obtenerDatos();
  }



  barProductosVendidos() {
    setTimeout(() => {
      this.barChart = new Chart(this.barCharRef.nativeElement, {
        type: 'bar',
        data: {
          labels: this.labelsTablero,
          datasets: [{
            label: 'PRODUCTOS VENDIDOS POR CANTIDAD',
            data: this.datosTableroBar,
            backgroundColor: this.backgroundBar,
            borderColor: this.borderColorBar,
            borderWidth: 1
          }]
        },

      });
    }, 1000);
  }
  barProductosVendidoMonto() {
    setTimeout(() => {
      this.barChart = new Chart(this.barMontoCharRef.nativeElement, {
        type: 'bar',
        data: {
          labels: this.labelsTablero,
          datasets: [{
            label: 'PRODUCTOS VENDIDOS POR MONTO',
            data: this.datosTableroMontoBar,
            backgroundColor: this.backgroundBar,
            borderColor: this.borderColorBar,
            borderWidth: 1
          }]
        },

      });
    }, 1000);
  }




  selectFechaIni(event) {
    this.fechaSeleccionadaIni = event;

  }

  selectFechaFin(event) {
    this.fechaSeleccionadaFin = event;

  }

  obtenerDatos() {


    let chartExist = Chart.getChart('barCanvas'); // <canvas> id
    if (chartExist !== undefined) {
      chartExist.destroy();
    }

    chartExist = Chart.getChart('doughnutCanvas'); // <canvas> id
    if (chartExist !== undefined) {
      chartExist.destroy();
    }


    chartExist = Chart.getChart('lineChartCanva'); // <canvas> id

    if (chartExist !== undefined) {
      chartExist.destroy();
    }

    this.listaVentasPorProducto = [];
    this.datosTablero = [];
    this.labelsTablero = [];
    this.datosTableroBar = [];
    this.datosTableroMontoBar = [];

    this.stockService.graficoVentaPorProducto(this.fechaSeleccionadaIni, this.fechaSeleccionadaFin).then((resulPromise) => {
      resulPromise.subscribe((resul) => {
        console.log(resul);
        this.listaVentasPorProducto = resul.listEntities;
        if (!this.listaVentasPorProducto) {
          this.stockService.showMessageWarning('No se tiene información para la fecha seleccionada');
          return;
        }

        //console.log('Datos Tablero', this.listaVentasPorProducto)

        // eslint-disable-next-line @typescript-eslint/prefer-for-of
        for (let i = 0; i < this.listaVentasPorProducto.length; i++) {
          //console.log('montos', this.listaVentasPorProducto[i].totalVenta)
          this.datosTablero.push(this.listaVentasPorProducto[i].totalVenta);
          this.labelsTablero.push(this.listaVentasPorProducto[i].producto);
          this.datosTableroBar.push(this.listaVentasPorProducto[i].cantidad);
          this.datosTableroMontoBar.push(this.listaVentasPorProducto[i].totalVenta);
        }

        //this.doughProductosVendidos();
        this.barProductosVendidos();
        this.barProductosVendidoMonto();
        //this.lineProductosVendidos();
        this.showTablero = true;
      });
    });
  }

  volverFechas() {
    //this.showTablero = false;
  }

}
