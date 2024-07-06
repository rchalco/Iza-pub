import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ComponentsModule } from 'src/app/components/components.module';
import { IonicModule } from '@ionic/angular';

import { AperturaInventarioPageRoutingModule } from './apertura-inventario-routing.module';

import { AperturaInventarioPage } from './apertura-inventario.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AperturaInventarioPageRoutingModule,
    ComponentsModule
  ],
  declarations: [AperturaInventarioPage]
})
export class AperturaInventarioPageModule {}
