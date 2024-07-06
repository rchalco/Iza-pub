import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { DatosFacturaDTO } from 'src/app/interfaces/general/DatosFactura';
import { PersonaService } from 'src/app/services/persona.service';

@Component({
  selector: 'app-registro-cliente-fac',
  templateUrl: './registro-cliente-fac.component.html',
  styleUrls: ['./registro-cliente-fac.component.scss'],
})
export class RegistroClienteFacComponent implements OnInit {
  @Output() public emiterConfirmacion = new EventEmitter<any>();
  @Output() public emiterCancelar = new EventEmitter<any>();

  listaTipoDocumento = [
    { idTipoDocumento: 1, documento: 'CI' },
    { idTipoDocumento: 2, documento: 'Doc Extrangero' },
    { idTipoDocumento: 3, documento: 'NIT' },
  ];

  currentDocument: any;
  clienteFactura: any = new Object();

  constructor(private personaService: PersonaService) {}

  ngOnInit() {}

  tipoDocumnetoSeleccionada(event) {
    this.currentDocument = event.detail.value;
  }
  regitrarCliente() {
    this.personaService
      .registrarClientreFactura(this.clienteFactura)
      .then((rsulService) => {
        rsulService.subscribe((resul) => {
          console.log('resul registro cliente', resul);
          this.clienteFactura.idclienteFac = resul.object;
        });
      });
    if (this.emiterConfirmacion) {
      this.emiterConfirmacion.emit(this.clienteFactura);
    }
  }
  cancelarRegistro() {
    if (this.emiterCancelar) {
      this.emiterCancelar.emit(this.clienteFactura);
    }
  }
}
