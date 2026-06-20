import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ReporteVentasProcesoPageRoutingModule } from './reporte-ventas-proceso-routing.module';

import { ReporteVentasProcesoPage } from './reporte-ventas-proceso.page';
import { ComponentsModule } from 'src/app/components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ReporteVentasProcesoPageRoutingModule,
    ComponentsModule
  ],
  declarations: [ReporteVentasProcesoPage]
})
export class ReporteVentasProcesoPageModule {}
