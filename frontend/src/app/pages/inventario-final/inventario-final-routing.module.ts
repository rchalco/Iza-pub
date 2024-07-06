import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { InventarioFinalPage } from './inventario-final.page';

const routes: Routes = [
  {
    path: '',
    component: InventarioFinalPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InventarioFinalPageRoutingModule {}
