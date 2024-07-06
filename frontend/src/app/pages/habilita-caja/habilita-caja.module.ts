import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ComponentsModule } from 'src/app/components/components.module';
import { IonicModule } from '@ionic/angular';

import { HabilitaCajaPageRoutingModule } from './habilita-caja-routing.module';

import { HabilitaCajaPage } from './habilita-caja.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    HabilitaCajaPageRoutingModule,
    ComponentsModule
  ],
  declarations: [HabilitaCajaPage]
})
export class HabilitaCajaPageModule {}
