import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { PagosQrPageRoutingModule } from './pagos-qr-routing.module';

import { PagosQrPage } from './pagos-qr.page';
import { ComponentsModule } from 'src/app/components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    PagosQrPageRoutingModule,
    ComponentsModule
  ],
  declarations: [PagosQrPage]
})
export class PagosQrPageModule {}
