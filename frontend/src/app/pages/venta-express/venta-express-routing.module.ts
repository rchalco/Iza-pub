import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { VentaExpressPage } from './venta-express.page';

const routes: Routes = [
  {
    path: '',
    component: VentaExpressPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class VentaExpressPageRoutingModule {}
