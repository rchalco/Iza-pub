import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ConfigPrinterPage } from './config-printer.page';

describe('ConfigPrinterPage', () => {
  let component: ConfigPrinterPage;
  let fixture: ComponentFixture<ConfigPrinterPage>;

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfigPrinterPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
