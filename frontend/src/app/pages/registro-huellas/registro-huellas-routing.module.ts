import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RegistroHuellasPage } from './registro-huellas.page';

const routes: Routes = [
  {
    path: '',
    component: RegistroHuellasPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RegistroHuellasPageRoutingModule {}
