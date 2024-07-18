import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { DashboardProductosPageRoutingModule } from './dashboard-productos-routing.module';

import { DashboardProductosPage } from './dashboard-productos.page';
import { DxBulletModule, DxDataGridModule, DxTemplateModule } from 'devextreme-angular';
import { ComponentsModule } from 'src/app/components/components.module';
import { PipesModule } from 'src/app/pipes/pipes.module';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    DashboardProductosPageRoutingModule,
    ComponentsModule,
    PipesModule,
    DxDataGridModule,
    DxTemplateModule,
    DxBulletModule
  ],
  declarations: [DashboardProductosPage]
})
export class DashboardProductosPageModule {}
