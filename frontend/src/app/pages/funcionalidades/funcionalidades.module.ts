import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { FuncionalidadesPageRoutingModule } from './funcionalidades-routing.module';

import { FuncionalidadesPage } from './funcionalidades.page';
import { ComponentsModule } from 'src/app/components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    FuncionalidadesPageRoutingModule,
    ComponentsModule
  ],
  declarations: [FuncionalidadesPage]
})
export class FuncionalidadesPageModule {}
