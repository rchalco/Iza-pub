import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ReporteCierreGlobalPage } from './reporte-cierre-global.page';

const routes: Routes = [
  {
    path: '',
    component: ReporteCierreGlobalPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReporteCierreGlobalPageRoutingModule {}
