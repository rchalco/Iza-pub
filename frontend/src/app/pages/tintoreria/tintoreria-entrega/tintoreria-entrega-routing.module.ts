import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TintoreriaEntregaPage } from './tintoreria-entrega.page';

const routes: Routes = [
  {
    path: '',
    component: TintoreriaEntregaPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TintoreriaEntregaPageRoutingModule {}
