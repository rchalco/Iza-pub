import { Component, OnInit } from '@angular/core';

import { StockService } from 'src/app/services/stock.service';

@Component({
  selector: 'app-entradas-salidas',
  templateUrl: './entradas-salidas.page.html',
  styleUrls: ['./entradas-salidas.page.scss'],
})
export class EntradasSalidasPage implements OnInit {


  currentAsignacion;
  tipoAsignacion = [
    {
      id: 1,
      descripcion: 'ENTRADA',
    },
    {
      id: 2,
      descripcion: 'SALIDA',
    },
  ];

  motivoAsigna;
  totalAsigna = 0;



  constructor(private stockService: StockService) { }

  ngOnInit() {

  }

  registrarAsignacion(){

    console.log('datos graba1', this.totalAsigna);
    console.log('datos graba2', this.motivoAsigna);
    console.log('datos graba3', this.currentAsignacion);

    this.stockService
    .grabaEntradaSalida(this.totalAsigna, this.motivoAsigna, this.currentAsignacion)
    .then((productosService) => {
      productosService.subscribe((resul) => {
        this.stockService.showMessageResponse(resul);
       
      });
    });
  //if (this.listaInventario.length<=0)
  //  return;

    this.motivoAsigna = '';
    this.totalAsigna = 0;

  }

  selectTipoAsigna(event) {
    this.currentAsignacion = event.detail.value;
   
  }

  validarAsignacion() {
    //console.log('validarCantidadVenta', this.selectedProducto);
    let validCantidad = true;
    if (this.totalAsigna <= 0) {
      this.stockService.showMessageWarning(
        'El monto debe ser mayor a 0'
      );
      validCantidad = false;
    }
    return validCantidad;
  }

}
