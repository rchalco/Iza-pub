import { Component, OnInit } from '@angular/core';
import { SaldoCajaDTO } from 'src/app/interfaces/caja/SaldoCaja';
import { StockService } from 'src/app/services/stock.service';

@Component({
  selector: 'app-cierre-caja',
  templateUrl: './cierre-caja.page.html',
  styleUrls: ['./cierre-caja.page.scss'],
})
export class CierreCajaPage implements OnInit {
  ultimasCajasCompleto: SaldoCajaDTO[] = [];
  ultimasCajas: SaldoCajaDTO[] = [];
  cajaActual: SaldoCajaDTO;

  constructor(private stockService: StockService) {}

  ngOnInit() {
    this.cajaActual = new SaldoCajaDTO();
    this.stockService.ultimasCajas("CIERRE").then(resultpromise=>{
      resultpromise.subscribe((resul) => {
        console.log(resul);
        this.ultimasCajasCompleto = resul.listEntities;
        this.cajaActual = this.ultimasCajasCompleto.filter((x) => x.esCajaActual == true)[0];
        this.ultimasCajas = this.ultimasCajasCompleto.filter((x) => x.esCajaActual != true);
        console.log("*ULTIMAS CAJAS COMPLETO",this.ultimasCajasCompleto);
        console.log("*ULTIMAS CAJAS",this.ultimasCajas);
        console.log("*CAJA ACTUAL",this.cajaActual);
      });
    });
    /*
    console.log('loginxxxxxxx',this.ultimasCajas);
    for(let x of this.ultimasCajas)
    {
      console.log('caja',x);

      if ( x.esCajaActual == true)
      {
        this.fechaCierre = x.fechaCierre;
        this.saldoCierre = x.saldoCierre;

      }

    }
    */
  }

  cerrarCaja() {
    console.log('cajaActual', this.cajaActual);
    this.stockService
      .cierreCaja(this.cajaActual.idCaja, this.cajaActual.saldoUsuario, this.cajaActual.observacion)
      .then(resultPromise=>{
        resultPromise.subscribe((resul) => {
          this.stockService.showMessageResponse(resul);
  
          if (resul.state === 3) {console.log('error cierre caja');}
          else {
            //this.logidata.Password = '';
            //this.logidata.PasswordNuevo = '';
            //this.logidata.PasswordNuevoRe = '';
          }
          //console.log('login lof correcto');
  
          //console.log('objet', resul);
        });
      });
  }
}
