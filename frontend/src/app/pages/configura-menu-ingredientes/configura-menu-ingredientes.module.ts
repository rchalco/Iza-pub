import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ConfiguraMenuIngredientesPageRoutingModule } from './configura-menu-ingredientes-routing.module';

import { ConfiguraMenuIngredientesPage } from './configura-menu-ingredientes.page';
import { ComponentsModule } from 'src/app/components/components.module';
import { PipesModule } from 'src/app/pipes/pipes.module';
import { DxBulletModule, DxDataGridModule, DxTemplateModule, DxValidatorModule } from 'devextreme-angular';



@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ConfiguraMenuIngredientesPageRoutingModule,
    ComponentsModule,
    PipesModule,
    DxDataGridModule,
    DxTemplateModule,
    DxBulletModule,
    DxValidatorModule
   
  ],
  declarations: [ConfiguraMenuIngredientesPage]
})
export class ConfiguraMenuIngredientesPageModule {}
