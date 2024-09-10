import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomHeaderComponent } from './custom-header/custom-header.component';
import { IonicModule } from '@ionic/angular';
import { RouterModule } from '@angular/router';
import { MenuComponent } from './menu/menu.component';
import { BuscaProductoComponent } from './busca-producto/busca-producto.component';
import { PipesModule } from '../pipes/pipes.module';
import { FormaPagoComponent } from './forma-pago/forma-pago.component';
import { FormsModule } from '@angular/forms';
import { DatosFacturaComponent } from './datos-factura/datos-factura.component';
import { RegistroClienteFacComponent } from './registro-cliente-fac/registro-cliente-fac.component';
import { CustomCalendarComponent } from './custom-calendar/custom-calendar.component';
import { ListaProductoComponent } from './lista-producto/lista-producto.component';
import { ProductsSlidesComponent } from './products-slides/products-slides.component';
import { ReaderCardComponent } from './reader-card/reader-card.component';
import { CustomCameraComponent } from './custom-camera/custom-camera.component';
import { FingerCaptureComponent } from './finger-capture/finger-capture.component';
import { DxDrawerModule, DxListModule, DxToolbarModule } from 'devextreme-angular';


@NgModule({
  declarations: [
    CustomHeaderComponent,
    MenuComponent,
    BuscaProductoComponent,
    FormaPagoComponent,
    DatosFacturaComponent,
    RegistroClienteFacComponent,
    CustomCalendarComponent,
    ListaProductoComponent,
    ProductsSlidesComponent,
    ReaderCardComponent,
    CustomCameraComponent,
    FingerCaptureComponent
  ],
  exports: [
    CustomHeaderComponent,
    MenuComponent,
    BuscaProductoComponent,
    FormaPagoComponent,
    DatosFacturaComponent,
    RegistroClienteFacComponent,
    CustomCalendarComponent,
    ListaProductoComponent,
    ProductsSlidesComponent,
    ReaderCardComponent,
    CustomCameraComponent,
    FingerCaptureComponent
  ],
  imports: [CommonModule, IonicModule, RouterModule, PipesModule, FormsModule, DxDrawerModule, DxToolbarModule, DxListModule],
})
export class ComponentsModule { }
