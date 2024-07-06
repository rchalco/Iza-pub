import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AperturaInventarioPage } from './apertura-inventario.page';

const routes: Routes = [
  {
    path: '',
    component: AperturaInventarioPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AperturaInventarioPageRoutingModule {}
