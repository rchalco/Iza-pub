import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MovimientoStockPage } from './movimiento-stock.page';

const routes: Routes = [
  {
    path: '',
    component: MovimientoStockPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MovimientoStockPageRoutingModule {}
