import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TarjetaReadPage } from './tarjeta-read.page';

const routes: Routes = [
  {
    path: '',
    component: TarjetaReadPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TarjetaReadPageRoutingModule {}
