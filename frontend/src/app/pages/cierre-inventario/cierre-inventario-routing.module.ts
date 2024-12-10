import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CierreInventarioPage } from './cierre-inventario.page';

const routes: Routes = [
  {
    path: '',
    component: CierreInventarioPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CierreInventarioPageRoutingModule {}
