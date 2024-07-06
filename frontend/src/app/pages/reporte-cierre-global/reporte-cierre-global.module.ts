import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ReporteCierreGlobalPageRoutingModule } from './reporte-cierre-global-routing.module';

import { ReporteCierreGlobalPage } from './reporte-cierre-global.page';
import { ComponentsModule } from 'src/app/components/components.module';
import { PipesModule } from 'src/app/pipes/pipes.module';
import { DxBulletModule, DxDataGridModule, DxTemplateModule } from 'devextreme-angular';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ReporteCierreGlobalPageRoutingModule,
    ComponentsModule,
    PipesModule,
    DxDataGridModule,
    DxTemplateModule,
    DxBulletModule
  ],
  declarations: [ReporteCierreGlobalPage]
})
export class ReporteCierreGlobalPageModule {}
