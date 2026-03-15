import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { BandejaPedidosPageRoutingModule } from './bandeja-pedidos-routing.module';

import { BandejaPedidosPage } from './bandeja-pedidos.page';
import { ComponentsModule } from 'src/app/components/components.module';
import { PipesModule } from 'src/app/pipes/pipes.module';

import { DxBulletModule, DxButtonModule, DxDataGridModule, DxDateBoxModule, DxPopupModule, DxTemplateModule } from 'devextreme-angular';
import { DxiPopupToolbarItemModule } from 'devextreme-angular/ui/popup/nested';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    BandejaPedidosPageRoutingModule,
    ComponentsModule,
    PipesModule,
    DxDataGridModule,
    DxTemplateModule,
    DxBulletModule,
    DxDateBoxModule,
    DxPopupModule,
    DxButtonModule,
    DxiPopupToolbarItemModule
  ],
  declarations: [BandejaPedidosPage]
})
export class BandejaPedidosPageModule {}
