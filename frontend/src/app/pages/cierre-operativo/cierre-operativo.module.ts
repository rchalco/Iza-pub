import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { CierreOperativoPageRoutingModule } from './cierre-operativo-routing.module';

import { CierreOperativoPage } from './cierre-operativo.page';
import { PipesModule } from 'src/app/pipes/pipes.module';
import { ComponentsModule } from 'src/app/components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    CierreOperativoPageRoutingModule,
    PipesModule,
    ComponentsModule
  ],
  declarations: [CierreOperativoPage]
})
export class CierreOperativoPageModule {}
