import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ConfigPrinterPageRoutingModule } from './config-printer-routing.module';

import { ConfigPrinterPage } from './config-printer.page';
import { ComponentsModule } from 'src/app/components/components.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ComponentsModule,
    ConfigPrinterPageRoutingModule,
  ],
  declarations: [ConfigPrinterPage],
})
export class ConfigPrinterPageModule {}
