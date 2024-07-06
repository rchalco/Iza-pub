import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ComponentsModule } from 'src/app/components/components.module';
import { IonicModule } from '@ionic/angular';

import { EntradasSalidasPageRoutingModule } from './entradas-salidas-routing.module';

import { EntradasSalidasPage } from './entradas-salidas.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    EntradasSalidasPageRoutingModule,
    ComponentsModule
  ],
  declarations: [EntradasSalidasPage]
})
export class EntradasSalidasPageModule {}
