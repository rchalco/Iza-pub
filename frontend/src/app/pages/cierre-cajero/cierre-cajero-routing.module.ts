import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CierreCajeroPage } from './cierre-cajero.page';

const routes: Routes = [
  {
    path: '',
    component: CierreCajeroPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CierreCajeroPageRoutingModule {}
