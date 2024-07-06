import { Component, OnInit } from '@angular/core';
import { MDPedidosPorEntregar } from 'src/app/interfaces/tintoreria/ResponsePedidosEntregar';
import { PersonaService } from 'src/app/services/persona.service';
import { TintoreriaService } from 'src/app/services/tintoreria.service';

@Component({
  selector: 'app-reporte-caja',
  templateUrl: './reporte-caja.page.html',
  styleUrls: ['./reporte-caja.page.scss'],
})
export class ReporteCajaPage implements OnInit {
  listaPedidos: MDPedidosPorEntregar[];
  listaCajas: any[];
  textoBusacar = '';
  fechaInicio: Date = new Date();
  fechaFin: Date;
  dateValue2: Date;
  idCaja = 0;
  total = 0;
  montoApertura = 0;
  montoCierre = 0;
  constructor(private tintoreriaService: TintoreriaService) {}

  ngOnInit() {
    this.textoBusacar = '';
    this.cargarCajas();
  }

  buscarPedidosEstado() {
    this.cargarPedidos();
  }

  cargarPedidos() {
    this.total = 0;
    this.tintoreriaService
      .obtieneArqueoCaja(this.idCaja, this.fechaInicio, this.fechaFin)
      .then((productosService) => {
        productosService.subscribe((resul) => {
          console.log(resul);
          this.listaPedidos = resul.listEntities as MDPedidosPorEntregar[];
          this.listaPedidos.forEach((zz) => {
            this.total += zz.total;
            this.montoApertura = zz.montoApertura;
            this.montoCierre = zz.montoCierre;
          });
          console.log('pedidos', this.listaPedidos);
        });
      });
  }

  cargarCajas() {
    this.tintoreriaService.obtieneCajaUsuario().then((productosService) => {
      productosService.subscribe((resul) => {
        console.log(resul);
        this.listaCajas = resul.listEntities;
        console.log('cajas', this.listaPedidos);
      });
    });
  }

  buscarPedidos(event) {
    this.textoBusacar = event.detail.value;
  }
  formatDate(value) {
    console.log(value);
  }

  setDatetimeIni(value) {
    this.fechaInicio = value;
  }

  setDatetimeFin(value) {
    this.fechaFin = value;
  }
  setCaja(event) {
    this.idCaja = event.detail.value;
  }
}
