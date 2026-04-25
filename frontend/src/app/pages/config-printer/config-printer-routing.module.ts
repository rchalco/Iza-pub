import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ConfigPrinterPage } from './config-printer.page';

const routes: Routes = [
  {
    path: '',
    component: ConfigPrinterPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ConfigPrinterPageRoutingModule {}
