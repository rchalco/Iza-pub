import { HttpClient } from '@angular/common/http';
import { LoadingController } from '@ionic/angular';
import { ToastController } from '@ionic/angular';
import { DatabaseService } from './DatabaseService';

export class BaseService {
  static isLoading = false;

  constructor(
    public databaseService: DatabaseService,
    public httpClient: HttpClient,
    public loadingController: LoadingController,
    public toastController: ToastController
  ) { }

  public async presentLoader() {
    BaseService.isLoading = true;
    return await this.loadingController
      .create({
        duration: 5000,
        message: 'Procesando...',
      })
      .then((a) => {
        a.present().then(() => {
          console.log('presented');
          if (!BaseService.isLoading) {
            a.dismiss().then(() => console.log('abort presenting'));
          }
        });
      });
  }

  public async dismissLoader() {
    BaseService.isLoading = false;
    return await this.loadingController
      .dismiss()
      .then(() => console.log('dismissed'))
      .catch((error) => console.log('error en dismiss loader', error));
  }

  public async showMessageResponse(response) {
    let color = 'success';
    switch (response.state) {
      case 1:
        color = 'success';
        break;
      case 2:
        color = 'warning';
        break;
      case 3:
        color = 'danger';
        break;
    }

    const toast = await this.toastController.create({
      message: response.message,
      duration: 3000,
      position: 'top',
      color: color,
    });


    toast.present();
  }

  public async showMessageError(_message) {
    const toast = await this.toastController.create({
      message: _message,
      duration: 3000,
      position: 'top',
      color: 'danger',
    });
    toast.present();
  }

  public async showMessageWarning(_message) {
    const toast = await this.toastController.create({
      message: _message,
      duration: 3000,
      position: 'top',
      color: 'warning',
    });
    toast.present();
  }

  public async showMessageSucess(_message) {
    const toast = await this.toastController.create({
      message: _message,
      duration: 3000,
      position: 'top',
      color: 'success',
    });
    toast.present();
  }

  public getInfoEviroment() {
    return this.databaseService.getItem('enviroment');
  }

  public deleteSession() {
    return this.databaseService.setItem('enviroment', null);
  }
}
