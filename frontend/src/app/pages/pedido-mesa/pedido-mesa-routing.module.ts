import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PedidoMesaPage } from './pedido-mesa.page';

const routes: Routes = [
  {
    path: '',
    component: PedidoMesaPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PedidoMesaPageRoutingModule {}
