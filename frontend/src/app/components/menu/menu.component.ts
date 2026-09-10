import { SeguridadService } from './../../services/seguridad.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MenuController } from '@ionic/angular';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { environment, verionsApp } from 'src/environments/environment';
import { MenuGeneralDTO } from 'src/app/interfaces/general/MenuGeneral';
import { FeatureFlagsService } from 'src/app/services/feature-flags.service';

@Component({
  standalone: false,
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
})
export class MenuComponent implements OnInit, OnDestroy {
  appPages: MenuGeneralDTO[] = [];
  private readonly configPrinterMenuOption: MenuGeneralDTO = {
    idMenuOpcion: -1,
    title: 'Configurar impresora',
    url: '/config-printer',
    icon: 'print-outline',
  };
  /** Reporte de cobros por QR. Se oculta junto con la funcionalidad. */
  private readonly pagosQrMenuOption: MenuGeneralDTO = {
    idMenuOpcion: -2,
    title: 'Pagos QR',
    url: '/pagos-qr',
    icon: 'qr-code-outline',
  };
  /** Pantalla que controla los flags. Siempre visible: es la unica forma de reactivarlos. */
  private readonly funcionalidadesMenuOption: MenuGeneralDTO = {
    idMenuOpcion: -3,
    title: 'Funcionalidades',
    url: '/funcionalidades',
    icon: 'options-outline',
  };
  version = verionsApp;
  usuario = environment.UsuarioLabel || environment.Usuario;
  rol = environment.rol;

  /** Menu que devolvio el backend, sin las opciones locales. */
  private menuBase: MenuGeneralDTO[] = [];
  private suscripcionFlags: Subscription = null;

  constructor(
    private baseService: SeguridadService,
    private menuCtrl: MenuController,
    private router: Router,
    private http: HttpClient,
    private featureFlags: FeatureFlagsService
  ) {}

  ngOnInit() {
    // Rearma el menu cuando cambian los flags, para no obligar a reiniciar la app
    // despues de apagar o encender una funcionalidad.
    this.suscripcionFlags = this.featureFlags.cambios$.subscribe(() => {
      this.appPages = this.addDefaultMenuOptions(this.menuBase);
    });
    this.featureFlags.cargar();
    this.initMenu();
  }

  ngOnDestroy() {
    this.suscripcionFlags?.unsubscribe();
  }

  initMenu() {
    this.usuario = environment.UsuarioLabel || environment.Usuario;
    this.rol = environment.rol;
    this.baseService.obtieneMenuPorUsuario().then((resulPromise) => {
      resulPromise.subscribe((resul) => {
        this.menuBase = resul.listEntities || [];
        this.appPages = this.addDefaultMenuOptions(this.menuBase);
        console.log('menu por usuario', resul.listEntities);
      });
    });
  }

  private addDefaultMenuOptions(menuItems: MenuGeneralDTO[]): MenuGeneralDTO[] {
    const opciones = [...menuItems];

    // Estas opciones no viven en el menu de la base: se agregan aqui.
    if (
      this.featureFlags.pagoQr &&
      !this.tieneOpcion(opciones, this.pagosQrMenuOption)
    ) {
      opciones.push(this.pagosQrMenuOption);
    }

    if (!this.tieneOpcion(opciones, this.funcionalidadesMenuOption)) {
      opciones.push(this.funcionalidadesMenuOption);
    }

    if (!this.tieneOpcion(opciones, this.configPrinterMenuOption)) {
      opciones.push(this.configPrinterMenuOption);
    }

    return opciones;
  }

  private tieneOpcion(
    menuItems: MenuGeneralDTO[],
    opcion: MenuGeneralDTO
  ): boolean {
    return menuItems.some((item) => item.url === opcion.url);
  }

  async cerrarSesion() {
    this.baseService.clearMenuCache();
    await this.menuCtrl.close('custom');
    this.router.navigate(['/login']);
  }
}
