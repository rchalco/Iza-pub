import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ReporteVentasConsolidadoPage } from './reporte-ventas-consolidado.page';

const routes: Routes = [
  {
    path: '',
    component: ReporteVentasConsolidadoPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReporteVentasConsolidadoPageRoutingModule {}
