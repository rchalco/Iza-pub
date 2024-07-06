import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';
import { ComponentsModule } from 'src/app/components/components.module';
import { CierreCajaPageRoutingModule } from './cierre-caja-routing.module';

import { CierreCajaPage } from './cierre-caja.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    CierreCajaPageRoutingModule,
    ComponentsModule
  ],
  declarations: [CierreCajaPage]
})
export class CierreCajaPageModule {}
