import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { PedidoMesaPageRoutingModule } from './pedido-mesa-routing.module';
import { ComponentsModule } from 'src/app/components/components.module';
import { PedidoMesaPage } from './pedido-mesa.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    PedidoMesaPageRoutingModule,
    ComponentsModule
  ],
  declarations: [PedidoMesaPage]
})
export class PedidoMesaPageModule {}
