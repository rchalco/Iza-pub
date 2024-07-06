import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PipesModule } from 'src/app/pipes/pipes.module';

import { CompraPage } from './compra.page';

const routes: Routes = [
  {
    path: '',
    component: CompraPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes), PipesModule],
  exports: [RouterModule],
})
export class CompraPageRoutingModule {}
