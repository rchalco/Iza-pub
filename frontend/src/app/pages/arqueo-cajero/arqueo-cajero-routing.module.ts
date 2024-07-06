import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ArqueoCajeroPage } from './arqueo-cajero.page';

const routes: Routes = [
  {
    path: '',
    component: ArqueoCajeroPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ArqueoCajeroPageRoutingModule {}
