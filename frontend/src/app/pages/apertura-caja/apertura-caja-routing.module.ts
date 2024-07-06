import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AperturaCajaPage } from './apertura-caja.page';

const routes: Routes = [
  {
    path: '',
    component: AperturaCajaPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AperturaCajaPageRoutingModule {}
