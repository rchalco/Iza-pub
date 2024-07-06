import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ConfiguracionItemMenuPageRoutingModule } from './configuracion-item-menu-routing.module';

import { ConfiguracionItemMenuPage } from './configuracion-item-menu.page';
import { ComponentsModule } from 'src/app/components/components.module';
import { PipesModule } from 'src/app/pipes/pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ConfiguracionItemMenuPageRoutingModule,
    ComponentsModule,
    PipesModule
  ],
  declarations: [ConfiguracionItemMenuPage]
})
export class ConfiguracionItemMenuPageModule { }
