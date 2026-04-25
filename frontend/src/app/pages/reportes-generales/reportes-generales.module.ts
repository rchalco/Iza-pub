import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ReportesGeneralesPageRoutingModule } from './reportes-generales-routing.module';

import { ReportesGeneralesPage } from './reportes-generales.page';
import { DxBulletModule, DxButtonModule, DxDataGridModule, DxDateBoxModule, DxPopupModule, DxSelectBoxModule, DxTabsModule, DxTemplateModule } from 'devextreme-angular';
import { DxiPopupToolbarItemModule } from 'devextreme-angular/ui/popup/nested';
import { PipesModule } from 'src/app/pipes/pipes.module';
import { ComponentsModule } from 'src/app/components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ReportesGeneralesPageRoutingModule,
    ComponentsModule,
    PipesModule,
    DxDataGridModule,
    DxTemplateModule,
    DxBulletModule,
    DxDateBoxModule,
    DxPopupModule,
    DxButtonModule,
    DxiPopupToolbarItemModule,
    DxSelectBoxModule,
    DxTabsModule,
  ],
  declarations: [ReportesGeneralesPage]
})
export class ReportesGeneralesPageModule {}
