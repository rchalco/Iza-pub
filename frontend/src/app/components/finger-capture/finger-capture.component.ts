import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FingerService } from 'src/app/services/finger.service';

@Component({
  selector: 'app-finger-capture',
  templateUrl: './finger-capture.component.html',
  styleUrls: ['./finger-capture.component.scss'],
})
export class FingerCaptureComponent implements OnInit {
  
@Output() public getValueEmmiter: EventEmitter<any> = new EventEmitter<any>();

  huella = "";
  constructor(
    private fingerService: FingerService
  ) { }

  ngOnInit() { }

  capturarHuella() {
    this.fingerService.capturarHuellaEnrrolar(5000).then(service => {
      service.subscribe(resul => {
        console.log('resultado huella', resul);
        this.huella = resul.CaptureFingerForEnrollResult.Message;
        
        if (this.getValueEmmiter) {
          this.getValueEmmiter.emit(this.huella);
        }
      });
    });
    
  }
}
