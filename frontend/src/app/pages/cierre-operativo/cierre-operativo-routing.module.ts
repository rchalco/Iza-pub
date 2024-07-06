import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CierreOperativoPage } from './cierre-operativo.page';

const routes: Routes = [
  {
    path: '',
    component: CierreOperativoPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CierreOperativoPageRoutingModule {}
