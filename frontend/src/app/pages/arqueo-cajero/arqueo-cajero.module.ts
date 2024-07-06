import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ArqueoCajeroPageRoutingModule } from './arqueo-cajero-routing.module';

import { ArqueoCajeroPage } from './arqueo-cajero.page';
import { ComponentsModule } from 'src/app/components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ArqueoCajeroPageRoutingModule,
    ComponentsModule
  ],
  declarations: [ArqueoCajeroPage]
})
export class ArqueoCajeroPageModule {}
