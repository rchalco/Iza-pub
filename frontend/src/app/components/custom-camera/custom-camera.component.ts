import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Camera, CameraResultType } from '@capacitor/camera';
import { SeguridadService } from 'src/app/services/seguridad.service';
import { ToastController } from '@ionic/angular';


@Component({
  standalone: false,
  selector: 'app-custom-camera',
  templateUrl: './custom-camera.component.html',
  styleUrls: ['./custom-camera.component.scss'],
})
export class CustomCameraComponent implements OnInit {
  @Output() public eventPhoto: EventEmitter<any> = new EventEmitter<any>();
  clickedImage: string;


  constructor(baseService: SeguridadService, private toastController: ToastController) { }

  pickPhotos = async () => {
    const photo = await Camera.getPhoto({
      quality: 20,
      allowEditing: true,
      resultType: CameraResultType.Base64
    }).then((imageData) => {
      console.log('************* resul foto', imageData.base64String);
      const base64Image = 'data:image/jpeg;base64,' + imageData.base64String;
      this.clickedImage = base64Image;
      this.eventPhoto.emit(base64Image);
    }, async (err) => {
      console.error(err);
      const toast = await this.toastController.create({
        message: 'Error al capturar la foto: ' + (err.message || 'Error desconocido'),
        duration: 3000,
        position: 'top',
        color: 'danger',
      });
      toast.present();
    });
    return photo;
  };
  ngOnInit() { }

}
