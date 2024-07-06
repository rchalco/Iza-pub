import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { TarjetaReadPageRoutingModule } from './tarjeta-read-routing.module';

import { TarjetaReadPage } from './tarjeta-read.page';
import { ComponentsModule } from 'src/app/components/components.module';
import { PipesModule } from 'src/app/pipes/pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    TarjetaReadPageRoutingModule,
    ComponentsModule,
    PipesModule
  ],
  declarations: [TarjetaReadPage]
})
export class TarjetaReadPageModule {}
