import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { InventarioGeneralPage } from './inventario-general.page';

const routes: Routes = [
  {
    path: '',
    component: InventarioGeneralPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InventarioGeneralPageRoutingModule {}
