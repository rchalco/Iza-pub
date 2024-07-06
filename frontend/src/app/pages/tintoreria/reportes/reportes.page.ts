import { Component, OnInit } from '@angular/core';
import { MDPedidosPorEntregar } from 'src/app/interfaces/tintoreria/ResponsePedidosEntregar';
import { PersonaService } from 'src/app/services/persona.service';
import { TintoreriaService } from 'src/app/services/tintoreria.service';

@Component({
  selector: 'app-reportes',
  templateUrl: './reportes.page.html',
  styleUrls: ['./reportes.page.scss'],
})
export class ReportesPage implements OnInit {
  listaPedidos: MDPedidosPorEntregar[];
  textoBusacar = '';
  fechaInicio: Date;
  fechaFin: Date;
  dateValue2: Date;
  estado = 0;
  constructor(private tintoreriaService: TintoreriaService) {}

  ngOnInit() {
    this.textoBusacar = '';
  }

  buscarPedidosEstado() {
    this.cargarPedidos();
  }

  cargarPedidos() {
    this.tintoreriaService
      .obtienePedidosReporte(this.estado, this.fechaInicio, this.fechaFin)
      .then((productosService) => {
        productosService.subscribe((resul) => {
          console.log(resul);
          this.listaPedidos = resul.listEntities as MDPedidosPorEntregar[];
          console.log('pedidos', this.listaPedidos);
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
  setEstado(event) {
    this.estado = event.detail.value;
  }
}
