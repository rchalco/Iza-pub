import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { FuncionalidadesPage } from './funcionalidades.page';

const routes: Routes = [
  {
    path: '',
    component: FuncionalidadesPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FuncionalidadesPageRoutingModule {}
