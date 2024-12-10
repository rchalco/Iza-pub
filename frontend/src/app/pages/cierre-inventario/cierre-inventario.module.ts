import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { CierreInventarioPageRoutingModule } from './cierre-inventario-routing.module';

import { CierreInventarioPage } from './cierre-inventario.page';
import { ComponentsModule } from 'src/app/components/components.module';
import { PipesModule } from 'src/app/pipes/pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    CierreInventarioPageRoutingModule,
    ComponentsModule,
    PipesModule
  ],
  declarations: [CierreInventarioPage]
})
export class CierreInventarioPageModule {}
