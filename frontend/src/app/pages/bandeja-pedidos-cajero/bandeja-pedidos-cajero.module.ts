import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { BandejaPedidosCajeroPageRoutingModule } from './bandeja-pedidos-cajero-routing.module';

import { BandejaPedidosCajeroPage } from './bandeja-pedidos-cajero.page';
import { ComponentsModule } from 'src/app/components/components.module';
import { PipesModule } from 'src/app/pipes/pipes.module';

import {
  DxBulletModule,
  DxButtonModule,
  DxDataGridModule,
  DxDateBoxModule,
  DxPopupModule,
  DxTemplateModule,
} from 'devextreme-angular';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    BandejaPedidosCajeroPageRoutingModule,
    ComponentsModule,
    PipesModule,
    DxDataGridModule,
    DxTemplateModule,
    DxBulletModule,
    DxDateBoxModule,
    DxPopupModule,
    DxButtonModule,
  ],
  declarations: [BandejaPedidosCajeroPage],
})
export class BandejaPedidosCajeroPageModule {}
