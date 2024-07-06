import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Camera, CameraResultType } from '@capacitor/camera';
import { SeguridadService } from 'src/app/services/seguridad.service';


@Component({
  selector: 'app-custom-camera',
  templateUrl: './custom-camera.component.html',
  styleUrls: ['./custom-camera.component.scss'],
})
export class CustomCameraComponent implements OnInit {
  @Output() public eventPhoto: EventEmitter<any> = new EventEmitter<any>();
  clickedImage: string;


  constructor(baseService: SeguridadService) { }

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
    }, (err) => {
      console.error(err);
    });
    return photo;
  };
  ngOnInit() { }

}
