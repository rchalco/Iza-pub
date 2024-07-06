import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ComponentsModule } from 'src/app/components/components.module';
import { IonicModule } from '@ionic/angular';

import { AsignacionInventarioPageRoutingModule } from './asignacion-inventario-routing.module';

import { AsignacionInventarioPage } from './asignacion-inventario.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AsignacionInventarioPageRoutingModule,
    ComponentsModule
  ],
  declarations: [AsignacionInventarioPage]
})
export class AsignacionInventarioPageModule {}
