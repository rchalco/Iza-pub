import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AbmUsuarioPage } from './abm-usuario.page';

const routes: Routes = [
  {
    path: '',
    component: AbmUsuarioPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AbmUsuarioPageRoutingModule {}
