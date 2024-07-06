import { AfterViewInit, Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';

@Component({
  selector: 'app-reader-card',
  templateUrl: './reader-card.component.html',
  styleUrls: ['./reader-card.component.scss'],
})
export class ReaderCardComponent implements OnInit, AfterViewInit {
  @ViewChild('cardInput') cardInput;
  @Output() public readCard: EventEmitter<string> =
    new EventEmitter<string>();
  carNumber = '';
  showCardInput = true;

  constructor() { }

  ngOnInit() {

  }

  public initReadCard() {
    this.carNumber = '';

    setTimeout(() => {
      this.showCardInput = true;
      this.cardInput.setFocus();
      //console.log('se inicio el foco del input', this.cardInput);
    }, 500);
  }

  eventChangeInput(event) {
    if (this.carNumber.length > 0) {
      //console.log('evento change tarjeta', this.carNumber);
      this.readCard?.emit(this.carNumber);
      this.carNumber = '';
      //this.showCardInput = false;
    }
  }

  ngAfterViewInit() {

  }

}
