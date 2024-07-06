import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ConfiguracionItemMenuPage } from './configuracion-item-menu.page';

const routes: Routes = [
  {
    path: '',
    component: ConfiguracionItemMenuPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ConfiguracionItemMenuPageRoutingModule {}
