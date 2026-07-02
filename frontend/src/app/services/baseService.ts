import { HttpClient } from '@angular/common/http';
import { LoadingController, ToastController } from '@ionic/angular';
import { DatabaseService } from './DatabaseService';
import notify from 'devextreme/ui/notify';

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

  public showMessageError(_message: string) {
    notify(
      {
        message: '',
        position: { my: 'center top', at: 'center top' },
        wrapperClass: 'toast-error-dx',
        displayTime: 5000,
        contentTemplate: (container: HTMLElement) => {
          container.innerHTML = _message;
        },
      },
      'error',
      5000,
    );
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

  public extractErrorMessage(error: any): string {
    if (error.status === 0) {
      const detail = this.htmlEncode(error.message || 'No se pudo establecer conexión con el servidor');
      return `<div class="toast-error-body">
  <div class="toast-error-row">
    <span class="toast-error-icon" aria-hidden="true">⚠</span>
    <span class="toast-error-main">La red está caída o el servidor está desconectado</span>
  </div>
  <div class="toast-error-detail">${detail}</div>
</div>`;
    }
    if (error.error?.message) {
      return error.error.message;
    }
    if (typeof error.error === 'string') {
      return error.error;
    }
    if (error.message) {
      return error.message;
    }
    return 'Error de comunicación con el servidor';
  }

  private htmlEncode(value: string): string {
    const div = document.createElement('div');
    div.appendChild(document.createTextNode(value));
    return div.innerHTML;
  }
}
