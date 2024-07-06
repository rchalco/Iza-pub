import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CierreGlobalPage } from './cierre-global.page';

const routes: Routes = [
  {
    path: '',
    component: CierreGlobalPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CierreGlobalPageRoutingModule {}
