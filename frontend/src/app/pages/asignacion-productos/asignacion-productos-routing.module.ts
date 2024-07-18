import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AsignacionProductosPage } from './asignacion-productos.page';

const routes: Routes = [
  {
    path: '',
    component: AsignacionProductosPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AsignacionProductosPageRoutingModule {}
