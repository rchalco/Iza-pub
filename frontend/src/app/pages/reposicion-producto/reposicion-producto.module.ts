import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ReposicionProductoPageRoutingModule } from './reposicion-producto-routing.module';

import { ReposicionProductoPage } from './reposicion-producto.page';
import { DxBulletModule, DxDataGridModule, DxTemplateModule } from 'devextreme-angular';
import { ComponentsModule } from 'src/app/components/components.module';
import { PipesModule } from 'src/app/pipes/pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ReposicionProductoPageRoutingModule,
    ComponentsModule,
    PipesModule,
    DxDataGridModule,
    DxTemplateModule,
    DxBulletModule,
  ],
  declarations: [ReposicionProductoPage]
})
export class ReposicionProductoPageModule {}
