import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { BandejaPedidosCajeroPage } from './bandeja-pedidos-cajero.page';

const routes: Routes = [
  {
    path: '',
    component: BandejaPedidosCajeroPage,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BandejaPedidosCajeroPageRoutingModule {}
