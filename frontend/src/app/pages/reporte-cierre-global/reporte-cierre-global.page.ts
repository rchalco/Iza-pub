import { Component, OnInit } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import { ResponseCierreGlobal } from 'src/app/interfaces/venta/cierreGlobal';
import { ResponseCierreGlobalAgrupado } from 'src/app/interfaces/venta/cierreGlobalAgrupado';
import { ResponseCierreGlobalDetalle } from 'src/app/interfaces/venta/cierreGlobalDetalle';

import { VentaService } from 'src/app/services/venta.service';

@Component({
  selector: 'app-reporte-cierre-global',
  templateUrl: './reporte-cierre-global.page.html',
  styleUrls: ['./reporte-cierre-global.page.scss'],
})
export class ReporteCierreGlobalPage implements OnInit {

  repoteGlobalDominium = new ResponseCierreGlobalAgrupado();
  listaGeneral: ResponseCierreGlobal[] = [];
  listaCover: ResponseCierreGlobalDetalle[] = [];
  listaCortesia: ResponseCierreGlobalDetalle[] = [];

  dataSourceGlobal: DataSource = new DataSource(this.listaGeneral);
  dataSourceCover: DataSource = new DataSource(this.listaCover);
  dataSourceCortesia: DataSource = new DataSource(this.listaCortesia);

  constructor(private ventaService: VentaService) { }

  ngOnInit() {

    this.ventaService.reporteCierreDominium().then((service) => {
      service.subscribe((response) => {
        if (response.state !== 1) {
          this.ventaService.showMessageResponse(response);
        }
        console.log('response', response);
        
        this.repoteGlobalDominium = response.object;
        
        this.listaGeneral= response.object.listaGeneral;
        this.dataSourceGlobal = response.object.listaGeneral;

        this.listaCover= response.object.listaCover;
        this.dataSourceCover = response.object.listaCover;

        this.listaCortesia= response.object.listaCortesia;
        this.dataSourceCortesia = response.object.listaCortesia;


        console.log('reporte', this.repoteGlobalDominium);
        console.log('cortesia', this.listaCortesia);
      });
    });


  }

}
