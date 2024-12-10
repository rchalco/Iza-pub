import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { VentaBusquedaPage } from './venta-busqueda.page';

const routes: Routes = [
  {
    path: '',
    component: VentaBusquedaPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class VentaBusquedaPageRoutingModule {}
