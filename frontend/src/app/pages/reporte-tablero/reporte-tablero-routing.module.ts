import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ReporteTableroPage } from './reporte-tablero.page';

const routes: Routes = [
  {
    path: '',
    component: ReporteTableroPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReporteTableroPageRoutingModule {}
