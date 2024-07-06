import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';


import { IonicModule } from '@ionic/angular';

import { ReporteTableroPageRoutingModule } from './reporte-tablero-routing.module';
import { ComponentsModule } from 'src/app/components/components.module';
import { ReporteTableroPage } from './reporte-tablero.page';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ReporteTableroPageRoutingModule,
    ComponentsModule
  ],
  declarations: [ReporteTableroPage]
})
export class ReporteTableroPageModule {}
