import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ControlTarjetasPageRoutingModule } from './control-tarjetas-routing.module';

import { ControlTarjetasPage } from './control-tarjetas.page';
import { ComponentsModule } from 'src/app/components/components.module';
import { PipesModule } from 'src/app/pipes/pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ControlTarjetasPageRoutingModule,
    ComponentsModule,
    PipesModule
  ],
  declarations: [ControlTarjetasPage]
})
export class ControlTarjetasPageModule {}
