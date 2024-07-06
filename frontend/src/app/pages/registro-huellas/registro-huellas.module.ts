import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { RegistroHuellasPageRoutingModule } from './registro-huellas-routing.module';

import { RegistroHuellasPage } from './registro-huellas.page';
import { ComponentsModule } from 'src/app/components/components.module';
import { PipesModule } from 'src/app/pipes/pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    RegistroHuellasPageRoutingModule,
    ComponentsModule,
    PipesModule
  ],
  declarations: [RegistroHuellasPage]
})
export class RegistroHuellasPageModule {}
