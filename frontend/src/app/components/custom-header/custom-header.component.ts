import { StockService } from 'src/app/services/stock.service';
import { Component, Input, OnInit } from '@angular/core';
import { MenuController, NavController } from '@ionic/angular';
import { environment } from 'src/environments/environment';
import { AppComponent } from 'src/app/app.component';

@Component({
  selector: 'app-custom-header',
  templateUrl: './custom-header.component.html',
  styleUrls: ['./custom-header.component.scss'],
})
export class CustomHeaderComponent implements OnInit {
  @Input() title: string;
  usuarioNombre: string;
  fechaProcesoActual: string;
  constructor(
    private menu: MenuController,
    private baseServices: StockService,
    private navCtri: NavController,
    private stockService: StockService,
    private appComponent: AppComponent
  ) {}
  ngOnInit() {
    this.stockService.getInfoEviroment().then((resul) => {
      console.log('env data header', resul);
      this.usuarioNombre = resul.Usuario;
      var fecha =  new Date(resul.fechaProceso);
      var year  = fecha.getFullYear();
      var month = (fecha.getMonth() + 1).toString().padStart(2, "0");
      var day   = fecha.getDate().toString().padStart(2, "0");
      this.fechaProcesoActual = year +'/'+ month +'/'+ day;
    });
  }
  showMenu() {
    //console.log("showMenu");
    this.menu.enable(true, 'custom');
    this.menu.open('custom').then((resul) => {
      console.log(resul);
    });
  }
  logout() {
    this.baseServices.deleteSession();
    this.menu.enable(false, 'custom');
    this.navCtri.navigateRoot('login');
    this.appComponent.disabledMenu();
  }

  goHome() {
    //this.baseServices.deleteSession();
    //this.menu.enable(false, 'custom');
    this.navCtri.navigateRoot('home');
    //this.appComponent.disabledMenu();
  }
}
