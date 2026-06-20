import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ReporteVentasProcesoPage } from './reporte-ventas-proceso.page';

const routes: Routes = [
  {
    path: '',
    component: ReporteVentasProcesoPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReporteVentasProcesoPageRoutingModule {}
