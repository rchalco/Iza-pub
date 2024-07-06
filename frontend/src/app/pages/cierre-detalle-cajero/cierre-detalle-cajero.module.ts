import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { CierreDetalleCajeroPageRoutingModule } from './cierre-detalle-cajero-routing.module';

import { CierreDetalleCajeroPage } from './cierre-detalle-cajero.page';
import { ComponentsModule } from 'src/app/components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    CierreDetalleCajeroPageRoutingModule,
    ComponentsModule
  ],
  declarations: [CierreDetalleCajeroPage]
})
export class CierreDetalleCajeroPageModule {}
