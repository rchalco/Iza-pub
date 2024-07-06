import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CierreDetalleCajeroPage } from './cierre-detalle-cajero.page';

const routes: Routes = [
  {
    path: '',
    component: CierreDetalleCajeroPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CierreDetalleCajeroPageRoutingModule {}
