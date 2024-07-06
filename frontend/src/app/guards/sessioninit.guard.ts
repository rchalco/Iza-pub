import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { NavController } from '@ionic/angular';
import { Observable } from 'rxjs';
import { StockService } from '../services/stock.service';

@Injectable({
  providedIn: 'root',
})
export class SessioninitGuard implements CanActivate {
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
    return this.verificarSession();
  }
  async verificarSession() {
    let resultVerfi = false;

    await this.baseService.getInfoEviroment().then((resul) => {
      //console.log('Guard SessioninitGuard: resultado de BD enviroment', resul);
      if (resul && resul.session > 0) {
        resultVerfi = true;
      } else {
        this.navCtri.navigateRoot('login');
        resultVerfi = false;
      }
    });
    return resultVerfi;
  }
}
