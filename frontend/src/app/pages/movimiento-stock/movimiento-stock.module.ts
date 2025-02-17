import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { MovimientoStockPageRoutingModule } from './movimiento-stock-routing.module';

import { MovimientoStockPage } from './movimiento-stock.page';
import { ComponentsModule } from 'src/app/components/components.module';
import { PipesModule } from 'src/app/pipes/pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    MovimientoStockPageRoutingModule,
    ComponentsModule,
    PipesModule,
  ],
  declarations: [MovimientoStockPage],
})
export class MovimientoStockPageModule {}
