import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { PersonasPageRoutingModule } from './personas-routing.module';
import { PersonasPage } from './personas.page';
import { ComponentsModule } from 'src/app/components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    PersonasPageRoutingModule,
    ComponentsModule
  ],
  declarations: [PersonasPage]
})
export class PersonasPageModule {}
