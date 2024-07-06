import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { VentaTintoreriaPageRoutingModule } from './venta-tintoreria-routing.module';

import { VentaTintoreriaPage } from './venta-tintoreria.page';
import { ComponentsModule } from 'src/app/components/components.module';
import { PipesModule } from 'src/app/pipes/pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    VentaTintoreriaPageRoutingModule,
    ComponentsModule,
    PipesModule
  ],
  declarations: [VentaTintoreriaPage]
})
export class VentaTintoreriaPageModule {}
