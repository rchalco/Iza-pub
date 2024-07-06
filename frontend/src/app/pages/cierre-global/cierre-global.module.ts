import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { CierreGlobalPageRoutingModule } from './cierre-global-routing.module';

import { CierreGlobalPage } from './cierre-global.page';
import { ComponentsModule } from 'src/app/components/components.module';
import { PipesModule } from 'src/app/pipes/pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    CierreGlobalPageRoutingModule,
    PipesModule,
    ComponentsModule,
  ],
  declarations: [CierreGlobalPage],
})
export class CierreGlobalPageModule {}
