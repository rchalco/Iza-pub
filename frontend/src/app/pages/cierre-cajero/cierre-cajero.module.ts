import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { CierreCajeroPageRoutingModule } from './cierre-cajero-routing.module';

import { CierreCajeroPage } from './cierre-cajero.page';
import { ComponentsModule } from 'src/app/components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    CierreCajeroPageRoutingModule,
    ComponentsModule
  ],
  declarations: [CierreCajeroPage]
})
export class CierreCajeroPageModule {}
