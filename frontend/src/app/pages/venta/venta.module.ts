import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { VentaPageRoutingModule } from './venta-routing.module';
import { ComponentsModule } from 'src/app/components/components.module';
import { VentaPage } from './venta.page';
import { PipesModule } from 'src/app/pipes/pipes.module';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    VentaPageRoutingModule,
    ComponentsModule,
    PipesModule
  ],
  declarations: [VentaPage]
})
export class VentaPageModule {}
