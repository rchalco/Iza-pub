import { SeguridadService } from './../../services/seguridad.service';
import { Component, OnInit } from '@angular/core';
import { MenuController } from '@ionic/angular';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment, verionsApp } from 'src/environments/environment';
import { MenuGeneralDTO } from 'src/app/interfaces/general/MenuGeneral';

@Component({
  standalone: false,
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
})
export class MenuComponent implements OnInit {
  appPages: MenuGeneralDTO[] = [];
  version = verionsApp;
  usuario = environment.UsuarioLabel || environment.Usuario;
  rol = environment.rol;

  constructor(
    private baseService: SeguridadService,
    private menuCtrl: MenuController,
    private router: Router,
    private http: HttpClient
  ) {}

  ngOnInit() {
    this.initMenu();
  }

  initMenu() {
    this.usuario = environment.UsuarioLabel || environment.Usuario;
    this.rol = environment.rol;
    this.baseService.obtieneMenuPorUsuario().then((resulPromise) => {
      resulPromise.subscribe((resul) => {
        this.appPages = resul.listEntities;
        console.log('menu por usuario', resul.listEntities);
      });
    });
  }

  async cerrarSesion() {
    this.baseService.clearMenuCache();
    await this.menuCtrl.close('custom');
    this.router.navigate(['/login']);
  }
}
