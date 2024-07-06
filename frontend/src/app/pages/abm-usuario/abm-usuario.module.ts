import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { AbmUsuarioPageRoutingModule } from './abm-usuario-routing.module';

import { AbmUsuarioPage } from './abm-usuario.page';
import { ComponentsModule } from 'src/app/components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AbmUsuarioPageRoutingModule,
    ComponentsModule,
  ],
  declarations: [AbmUsuarioPage],
})
export class AbmUsuarioPageModule {}
