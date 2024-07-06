import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HabilitaCajaPage } from './habilita-caja.page';

const routes: Routes = [
  {
    path: '',
    component: HabilitaCajaPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HabilitaCajaPageRoutingModule {}
