import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ReposicionProducto2Page } from './reposicion-producto2.page';

const routes: Routes = [
  {
    path: '',
    component: ReposicionProducto2Page
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReposicionProducto2PageRoutingModule {}
