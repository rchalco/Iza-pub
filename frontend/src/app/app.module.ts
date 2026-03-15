import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { ComponentsModule } from './components/components.module';
import { PipesModule } from './pipes/pipes.module';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { IonicStorageModule } from '@ionic/storage-angular';
import { Chart } from 'chart.js';
@NgModule({ declarations: [AppComponent],
    bootstrap: [AppComponent], imports: [BrowserModule,
        IonicModule.forRoot(),
        AppRoutingModule,
        ComponentsModule,
        PipesModule,
        IonicStorageModule.forRoot()], providers: [
        { provide: RouteReuseStrategy, useClass: IonicRouteStrategy },
        provideHttpClient(withInterceptorsFromDi()),
    ] })
export class AppModule {}
