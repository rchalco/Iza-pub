import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AsignacionInventarioPage } from './asignacion-inventario.page';

const routes: Routes = [
  {
    path: '',
    component: AsignacionInventarioPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AsignacionInventarioPageRoutingModule {}
