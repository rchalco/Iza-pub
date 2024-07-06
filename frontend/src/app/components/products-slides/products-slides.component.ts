import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-products-slides',
  templateUrl: './products-slides.component.html',
  styleUrls: ['./products-slides.component.scss'],
})
export class ProductsSlidesComponent implements OnInit {
  @Input() products;
  @Output() selectItem: EventEmitter<any>;
  @Input() textoBusacar = '';

  constructor() {}

  ngOnInit() {}
  clickItem(producto) {
    if (this.selectItem) {
      this.selectItem.emit(producto);
    }
  }
}
