import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { AperturaCajaPageRoutingModule } from './apertura-caja-routing.module';
import { ComponentsModule } from 'src/app/components/components.module';
import { AperturaCajaPage } from './apertura-caja.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AperturaCajaPageRoutingModule,
    ComponentsModule
  ],
  declarations: [AperturaCajaPage]
})
export class AperturaCajaPageModule {}
