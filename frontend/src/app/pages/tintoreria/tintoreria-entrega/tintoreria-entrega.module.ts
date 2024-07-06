import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { TintoreriaEntregaPageRoutingModule } from './tintoreria-entrega-routing.module';

import { TintoreriaEntregaPage } from './tintoreria-entrega.page';
import { ComponentsModule } from 'src/app/components/components.module';
import { PipesModule } from 'src/app/pipes/pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    TintoreriaEntregaPageRoutingModule,
    ComponentsModule,
    PipesModule,
  ],
  declarations: [TintoreriaEntregaPage],
})
export class TintoreriaEntregaPageModule {}
