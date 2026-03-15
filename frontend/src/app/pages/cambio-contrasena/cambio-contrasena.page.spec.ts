import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { IonicStorageModule } from '@ionic/storage-angular';
import { AppComponent } from 'src/app/app.component';
import { StockService } from 'src/app/services/stock.service';
import { CambioContrasenaPageModule } from './cambio-contrasena.module';

import { CambioContrasenaPage } from './cambio-contrasena.page';

const appComponentMock = { enabledMenu: false, initMenu: jest.fn() };
const stockServiceMock = {
  getInfoEviroment: jest.fn().mockResolvedValue({ Usuario: 'Test', fechaProceso: new Date().toISOString() }),
};

describe('CambioContrasenaPage', () => {
  let component: CambioContrasenaPage;
  let fixture: ComponentFixture<CambioContrasenaPage>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [CambioContrasenaPageModule, RouterModule.forRoot([]), HttpClientTestingModule, IonicStorageModule.forRoot()],
      providers: [
        { provide: AppComponent, useValue: appComponentMock },
        { provide: StockService, useValue: stockServiceMock },
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    }).compileComponents();

    fixture = TestBed.createComponent(CambioContrasenaPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
