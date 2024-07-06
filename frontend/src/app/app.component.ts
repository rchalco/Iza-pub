import { Component, ViewChild } from '@angular/core';
import { environment } from 'src/environments/environment';
import { MenuComponent } from './components/menu/menu.component';
@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  @ViewChild('menu') menu: MenuComponent;
  enabledMenu = false;
  constructor() { }

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
