import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';
import { ComponentsModule } from 'src/app/components/components.module';
import { ReportesPageRoutingModule } from './reportes-routing.module';
import { PipesModule } from 'src/app/pipes/pipes.module';
import { ReportesPage } from './reportes.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ReportesPageRoutingModule,
    ComponentsModule,
    PipesModule
  ],
  declarations: [ReportesPage]
})
export class ReportesPageModule {}
