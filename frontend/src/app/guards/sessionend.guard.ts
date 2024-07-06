import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { NavController } from '@ionic/angular';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { StockService } from '../services/stock.service';

@Injectable({
  providedIn: 'root',
})
export class SessionendGuard implements CanActivate {
  constructor(
    private baseService: StockService,
    private navCtri: NavController
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    const resul2 = this.verificarSession();
    console.log('resul2', !!resul2);
    return !!resul2;
  }

  async verificarSession() {
    let resultVerfi = false;

    await this.baseService.getInfoEviroment().then((resul) => {
      //console.log('Guard SessionendGuard: resultado de BD enviroment', resul);
      if (resul && resul.session > 0) {
        this.navCtri.navigateRoot('venta');
        resultVerfi = true;
      } else {
        resultVerfi = false;
      }
    });
    console.log('resultVerfi', resultVerfi);
    return resultVerfi;
  }
}
