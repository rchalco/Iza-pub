import { Component, OnInit } from '@angular/core';
import { ListaProductoComponent } from 'src/app/components/lista-producto/lista-producto.component';
import { UsuarioDTO } from 'src/app/interfaces/general/UsuarioDTO';
import { StockService } from 'src/app/services/stock.service';

@Component({
  selector: 'app-habilita-caja',
  templateUrl: './habilita-caja.page.html',
  styleUrls: ['./habilita-caja.page.scss'],
})
export class HabilitaCajaPage implements OnInit {

  detalleCajeros: UsuarioDTO[] = [];

  constructor(private stockService: StockService) { }

  ngOnInit() {

    this.stockService.obtieneListaCajeroCompleto().then((productosService) => {
      productosService.subscribe((resul) => {
        console.log(resul);
        this.detalleCajeros = resul.listEntities;
        
      });
    });
  }

  grabarCajeros(){
    console.log('CAjeros', this.detalleCajeros);

    this.stockService
      .actualizaCajerosActivos(
        this.detalleCajeros
      )
      .then((resul) => {
        resul.subscribe((x) => {
          this.stockService.showMessageResponse(x);
          
        });
      });
    
  }
}
