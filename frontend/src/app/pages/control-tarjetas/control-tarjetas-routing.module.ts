import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ControlTarjetasPage } from './control-tarjetas.page';

const routes: Routes = [
  {
    path: '',
    component: ControlTarjetasPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ControlTarjetasPageRoutingModule {}
