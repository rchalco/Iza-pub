import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { CambioContrasenaPageRoutingModule } from './cambio-contrasena-routing.module';
import { ComponentsModule } from 'src/app/components/components.module';
import { CambioContrasenaPage } from './cambio-contrasena.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    CambioContrasenaPageRoutingModule,
    ComponentsModule
  ],
  declarations: [CambioContrasenaPage]
})
export class CambioContrasenaPageModule {}
