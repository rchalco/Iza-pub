import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PagosQrPage } from './pagos-qr.page';

const routes: Routes = [
  {
    path: '',
    component: PagosQrPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PagosQrPageRoutingModule {}
