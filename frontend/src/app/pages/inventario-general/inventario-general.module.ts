import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ComponentsModule } from 'src/app/components/components.module';
import { IonicModule } from '@ionic/angular';

import { InventarioGeneralPageRoutingModule } from './inventario-general-routing.module';

import { InventarioGeneralPage } from './inventario-general.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    InventarioGeneralPageRoutingModule,
    ComponentsModule
  ],
  declarations: [InventarioGeneralPage]
})
export class InventarioGeneralPageModule {}
