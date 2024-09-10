import { SeguridadService } from './../../services/seguridad.service';
import { Component, OnInit, ChangeDetectorRef, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { StockService } from 'src/app/services/stock.service';
import { environment } from 'src/environments/environment';
import { timer, Subject } from 'rxjs';
import { map, takeUntil, takeWhile, finalize } from 'rxjs/operators';
import { MenuController } from '@ionic/angular';
import { HttpClient } from '@angular/common/http';
import { AnyObject } from 'chart.js/types/basic';
import { MenuGeneralDTO } from 'src/app/interfaces/general/MenuGeneral';
import { DxListModule, DxRadioGroupModule, DxToolbarModule } from 'devextreme-angular';
import { DxDrawerModule, DxDrawerComponent } from 'devextreme-angular/ui/drawer';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
})
export class MenuComponent implements OnInit {
  public appPages = [
    { title: 'Compras', url: '/compra', icon: 'paper-plane' },
    { title: 'Ventas', url: '/venta', icon: 'paper-plane' },
    { title: 'Pedido-Mesa', url: '/pedido-mesa', icon: 'paper-plane' },
    { title: 'Apertura Caja', url: '/apertura-caja', icon: 'paper-plane' },
    { title: 'Cierre Caja', url: '/cierre-caja', icon: 'paper-plane' },
    {
      title: 'Cambio Contraseña',
      url: '/cambio-contrasena',
      icon: 'paper-plane',
    },
    {
      title: 'Reportes y Tableros',
      url: '/reporte-tablero',
      icon: 'paper-plane',
    },
    {
      title: 'Venta Tintoreria',
      url: '/venta-tintoreria',
      icon: 'paper-plane',
    },
  ];

  menuPorUsuario: MenuGeneralDTO[] = [];
  isDrawerOpen = true;

  constructor(
    private baseService: SeguridadService,
    private menu: MenuController,
    private http: HttpClient
  ) {}

  toolbarContent = [{
    widget: 'dxButton',
    location: 'before',
    options: {
      icon: 'menu',
      stylingMode: 'text',
      onClick: () => this.isDrawerOpen = !this.isDrawerOpen,
    },
  }];

  ngOnInit() {
    this.initMenu();
  }

  initMenu() {
    //this.http.get('../../assets/data/menu-tintoreria.json').subscribe((res) => {

    this.baseService.obtieneMenuPorUsuario().then((resulPromise) => {
      resulPromise.subscribe((resul) => {
        console.log('Menu resultado', resul);
        this.appPages = resul.listEntities;
      });
    });

    // this.baseService.obtieneMenuPorUsuario1()
    // .subscribe((resul) => {
      
    //   console.log('Menu resultado', resul);
    //     this.appPages = resul.listEntities;
    // });
    console.log('Menu resultado 2', this.appPages);
    //this.menu.enable(false, 'custom');
  }
}
