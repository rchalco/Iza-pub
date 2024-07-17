/* eslint-disable @typescript-eslint/dot-notation */
import { Component, OnInit } from '@angular/core';
import { SaldoCajaDTO } from 'src/app/interfaces/caja/SaldoCaja';
import { DatabaseService } from 'src/app/services/DatabaseService';
import { StockService } from 'src/app/services/stock.service';
import { VentaService } from 'src/app/services/venta.service';
import { InventarioService } from 'src/app/services/inventario.service';
import { environment } from 'src/environments/environment';

//import { format, parseISO } from 'date-fns';

@Component({
  selector: 'app-apertura-caja',
  templateUrl: './apertura-caja.page.html',
  styleUrls: ['./apertura-caja.page.scss'],
})
export class AperturaCajaPage implements OnInit {
  cajaActual: SaldoCajaDTO;
  fechaSeleccionada: Date = new Date();
  barraCurrent;
  barras: any[] = [];

  constructor(
    private stockService: StockService,
    private databaseService: DatabaseService,
    private ventaService: VentaService,
    private inventarioService: InventarioService
  ) {}

  ngOnInit() {
    this.cajaActual = new SaldoCajaDTO();
    this.cajaActual.estadoCaja = 'PENDIENTE';
    this.cajaActual.fechaApertura = new Date();
    this.cajaActual.saldoCierre = 0;
    this.cajaActual.saldoInicial = 0;
    this.fechaSeleccionada = new Date();
    this.cargarBarras();
  }

  cargarBarras() {
    this.inventarioService.obtenerAlmacenes().then((resul) => {
      resul.subscribe((x) => {
        this.barras = x.listEntities;
        console.log('barras', this.barras);
      });
    });
  }

  aperturaCaja() {
    if (!this.barraCurrent) {
      this.stockService.showMessageWarning(
        'Debe Seleccionar una barra para continuar'
      );
      return;
    }
    if (this.cajaActual.estadoCaja === 'APERTURADA') {
      this.stockService.showMessageWarning(
        'La caja en la fecha seleccionada ya fue ABIERTA'
      );
      return;
    }

    if (this.cajaActual.estadoCaja === 'CERRADA') {
      this.stockService.showMessageError('No se puede abrir un caja CERRADA');
      return;
    }

    if (!this.barraCurrent) {
      this.stockService.showMessageError(
        'Debe seleccionar una barra aperturar una caja'
      );
      return;
    }

    this.stockService
      .aperturaCaja(
        this.fechaSeleccionada,
        this.cajaActual.saldoInicial,
        this.cajaActual.observacion,
        this.barraCurrent.idAlmacen
      )
      .then((resultPromise) => {
        resultPromise.subscribe((resul) => {
          this.stockService.showMessageResponse(resul);
          console.log('aperturaCaja: ', resul);
          if (resul.state === 1) {
            this.databaseService.getItem('enviroment').then((item) => {
              item['idOperacionDiariaCaja'] =
                environment.idOperacionDiariaCaja =
                  resul.object.idOperacionDiaria;
              item['idFechaProceso'] = environment.idFechaProceso =
                resul.object.idFechaProceso;
              item['fechaProceso'] = environment.fechaProceso =
                resul.object.fechaProceso;
              console.log('envNew', item);
              this.databaseService.setItem('enviroment', item);
            });
          }
        });
      });
  }

  selectBarra(event) {
    this.barraCurrent = event.detail.value;
    this.stockService.getInfoEviroment().then((x) => {
      x.idAlmacen = this.barraCurrent.idAlmacen;
      this.databaseService.setItem('enviroment', x);
    });
  }
}
