import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ClasificadorDTO } from 'src/app/interfaces/general/clasificador';
import { DatosFacturaDTO } from 'src/app/interfaces/general/DatosFactura';
import { StockService } from 'src/app/services/stock.service';

@Component({
  selector: 'app-datos-factura',
  templateUrl: './datos-factura.component.html',
  styleUrls: ['./datos-factura.component.scss'],
})
export class DatosFacturaComponent implements OnInit {

  listaTipoDocumento: ClasificadorDTO[] = [];
  selectTipoDocumento: ClasificadorDTO;
  datosFactura: DatosFacturaDTO;

  nroDocumento:string;
  nombreFactura: string;
  email:string;
  
  @Output() public datosFacturaFinal: EventEmitter<DatosFacturaDTO> =
    new EventEmitter<DatosFacturaDTO>();
    
  constructor(private stockService: StockService) { }

  ngOnInit() {
    this.stockService.clasificadorPorTipo(1).then((resulPromise) => {
      resulPromise.subscribe((resul) => {
        //console.log(resul);
        this.listaTipoDocumento = resul.listEntities;
        //console.log('INI FORMA PAGO', this.listaFormaPago)
      });
    });
    this.selectTipoDocumento = new ClasificadorDTO();
  }

  tipoDocumnetoSeleccionada(event){
    this.selectTipoDocumento.idClasificador = event.detail.value;
    ///Ver si se mostraran mascaras para el documento
  }
  documentoLostFocus(){
    ///Buscar NIT en BD
  }
  confirmarDatosFactura(){
    this.datosFactura = new DatosFacturaDTO();
    this.datosFactura.idcTipoDocumento = this.selectTipoDocumento.idClasificador;
    this.datosFactura.nombreFactura = this.nombreFactura;
    this.datosFactura.documento= this.nroDocumento;
    this.datosFactura.email = this.email;
    this.datosFacturaFinal.emit(this.datosFactura);
  }
}
