/* eslint-disable @typescript-eslint/member-ordering */
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { VentaService } from 'src/app/services/venta.service';

@Component({
  selector: 'app-forma-pago',
  templateUrl: './forma-pago.component.html',
  styleUrls: ['./forma-pago.component.scss'],
})
export class FormaPagoComponent implements OnInit {
  listFormasPago = [];
  @Input() montoTotal = 100;
  @Input() diferencia = 0;
  @Output() public eventCobrar: EventEmitter<any> = new EventEmitter<any>();
  @Output() public eventCancelar: EventEmitter<any> = new EventEmitter<any>();

  constructor(private ventaService: VentaService) {}

  ngOnInit() {
    this.ventaService.obtenerFormasDePago().then((x) => {
      x.subscribe((y) => {
        console.log('obtenerFormasDePago', y);
        this.listFormasPago = y.listEntities;
        if (this.listFormasPago && this.listFormasPago.length > 0) {
          this.listFormasPago[0].value = this.montoTotal;
        }
      });
    });
  }

  calcularDifrencia(event) {
    let suma = 0;
    this.listFormasPago.forEach((x) => {
      if (x.value) {
        suma += x.value;
      }
    });
    this.diferencia = this.montoTotal - suma;
  }

  cobrar() {
    if (this.diferencia !== 0) {
      this.ventaService.showMessageWarning(
        'El monto total no es igual a la suma de los metodos de pago!'
      );
      return;
    }

    if (this.eventCobrar) {
      this.eventCobrar.emit(this.listFormasPago);
    }
  }

  cancelar() {
    if (this.eventCancelar) {
      this.eventCancelar.emit();
    }
  }
}
