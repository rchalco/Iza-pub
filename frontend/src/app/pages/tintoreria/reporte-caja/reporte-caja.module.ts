import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';
import { ComponentsModule } from 'src/app/components/components.module';

import { ReporteCajaPageRoutingModule } from './reporte-caja-routing.module';
import { PipesModule } from 'src/app/pipes/pipes.module';
import { ReporteCajaPage } from './reporte-caja.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ReporteCajaPageRoutingModule,
    ComponentsModule,
    PipesModule
  ],
  declarations: [ReporteCajaPage]
})
export class ReporteCajaPageModule {}
