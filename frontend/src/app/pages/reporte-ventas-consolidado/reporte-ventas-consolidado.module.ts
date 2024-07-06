import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ReporteVentasConsolidadoPageRoutingModule } from './reporte-ventas-consolidado-routing.module';

import { ReporteVentasConsolidadoPage } from './reporte-ventas-consolidado.page';
import { ComponentsModule } from 'src/app/components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ReporteVentasConsolidadoPageRoutingModule,
    ComponentsModule
  ],
  declarations: [ReporteVentasConsolidadoPage]
})
export class ReporteVentasConsolidadoPageModule {}
