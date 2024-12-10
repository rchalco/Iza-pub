import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { VentaBusquedaPageRoutingModule } from './venta-busqueda-routing.module';

import { VentaBusquedaPage } from './venta-busqueda.page';
import { ComponentsModule } from 'src/app/components/components.module';
import { PipesModule } from 'src/app/pipes/pipes.module';
import { DxBulletModule, DxDataGridModule, DxTemplateModule, DxValidationGroupModule, DxValidatorModule } from 'devextreme-angular';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    VentaBusquedaPageRoutingModule,
    ComponentsModule,
    PipesModule,
    DxDataGridModule,
    DxTemplateModule,
    DxBulletModule,
    DxValidatorModule,
        DxValidationGroupModule
  ],
  declarations: [VentaBusquedaPage]
})
export class VentaBusquedaPageModule {}
