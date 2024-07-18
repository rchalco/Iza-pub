import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { AsignacionProductosPageRoutingModule } from './asignacion-productos-routing.module';

import { AsignacionProductosPage } from './asignacion-productos.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AsignacionProductosPageRoutingModule
  ],
  declarations: [AsignacionProductosPage]
})
export class AsignacionProductosPageModule {}
