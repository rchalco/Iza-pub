import { NgModule, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ComponentsModule } from 'src/app/components/components.module';

import { IonicModule } from '@ionic/angular';

import { AsignacionProductosPageRoutingModule } from './asignacion-productos-routing.module';

import { AsignacionProductosPage } from './asignacion-productos.page';
import { InventarioService } from 'src/app/services/inventario.service';
import { DxBulletModule, DxDataGridModule, DxTemplateModule } from 'devextreme-angular';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AsignacionProductosPageRoutingModule,
    ComponentsModule,
    DxBulletModule, DxDataGridModule, DxTemplateModule
  ],
  declarations: [AsignacionProductosPage]
})
export class AsignacionProductosPageModule {
}
