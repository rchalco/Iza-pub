import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ReportesGeneralesPage } from './reportes-generales.page';

const routes: Routes = [
  {
    path: '',
    component: ReportesGeneralesPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReportesGeneralesPageRoutingModule {}
