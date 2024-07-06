import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { VerificacionHuellaPageRoutingModule } from './verificacion-huella-routing.module';

import { VerificacionHuellaPage } from './verificacion-huella.page';
import { ComponentsModule } from 'src/app/components/components.module';
import { PipesModule } from 'src/app/pipes/pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    VerificacionHuellaPageRoutingModule,
    ComponentsModule,
    PipesModule
  ],
  declarations: [VerificacionHuellaPage]
})
export class VerificacionHuellaPageModule {}
