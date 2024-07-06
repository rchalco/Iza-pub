import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ReporteCajaPage } from './reporte-caja.page';

const routes: Routes = [
  {
    path: '',
    component: ReporteCajaPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReporteCajaPageRoutingModule {}
