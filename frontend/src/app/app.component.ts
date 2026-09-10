import { Component, ViewChild } from '@angular/core';
import { environment } from 'src/environments/environment';
import { MenuComponent } from './components/menu/menu.component';
import { FeatureFlagsService } from './services/feature-flags.service';
@Component({
  standalone: false,
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  @ViewChild('menu') menu: MenuComponent;
  enabledMenu = false;

  // Se precarga al arrancar porque quien decide el flujo de cobro lo lee de forma
  // sincrona: no puede esperar una lectura de disco en medio de una venta.
  constructor(private featureFlags: FeatureFlagsService) {
    this.featureFlags.cargar();
  }

  initMenu() {
    this.enabledMenu = true;
    setTimeout(() => {
      this.menu.initMenu();
    }, 1000);

  }
  disabledMenu() {
    this.enabledMenu = false;
  }
}
