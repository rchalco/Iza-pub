import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { VentaTintoreriaPage } from './venta-tintoreria.page';

const routes: Routes = [
  {
    path: '',
    component: VentaTintoreriaPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class VentaTintoreriaPageRoutingModule {}
