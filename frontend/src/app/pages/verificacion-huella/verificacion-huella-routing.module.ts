import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { VerificacionHuellaPage } from './verificacion-huella.page';

const routes: Routes = [
  {
    path: '',
    component: VerificacionHuellaPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class VerificacionHuellaPageRoutingModule {}
