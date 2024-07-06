/* eslint-disable radix */
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PopoverController } from '@ionic/angular';

@Component({
  selector: 'app-custom-calendar',
  templateUrl: './custom-calendar.component.html',
  styleUrls: ['./custom-calendar.component.scss'],
})
export class CustomCalendarComponent implements OnInit {
  @Input() public label: string;
  @Input() public id: string;
  @Input() public _value: Date = new Date();

  @Output() public getValueEmmiter: EventEmitter<any> = new EventEmitter<any>();
  valueDate = '';

  constructor(private popoverController: PopoverController) {}

  ngOnInit() {
    // eslint-disable-next-line no-underscore-dangle
    console.log('init calendar', this._value);
  }

  async setValue(event) {
    await this.popoverController.dismiss();
    console.log(event.detail.value);
    this.valueDate = event.detail.value.split('T')[0];
    const dia = this.valueDate.split('-')[2];
    const mes = this.valueDate.split('-')[1];
    const gestion = this.valueDate.split('-')[0];
    this.valueDate = dia + '/' + mes + '/' + gestion;
    const dateValue = new Date(
      parseInt(gestion),
      parseInt(mes) - 1,
      parseInt(dia),
      0,
      0,
      0,
      0
    );
    console.log(dateValue);
    if (this.getValueEmmiter) {
      this.getValueEmmiter.emit(dateValue);
    }
  }

  async setValueIni() {

  }
}
