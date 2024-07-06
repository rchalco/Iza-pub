import { Component, OnInit } from '@angular/core';
import { InventarioFinalDTO } from 'src/app/interfaces/inventario/InventarioFinal';
import { VentaService } from 'src/app/services/venta.service';
import {
  DxDataGridModule,
  DxBulletModule,
  DxTemplateModule,
} from 'devextreme-angular';
import DataSource from 'devextreme/data/data_source';
import DxDataGrid from 'devextreme/ui/data_grid';
//import { Workbook } from 'exceljs';
import { exportDataGrid } from 'devextreme/excel_exporter';
import { saveAs } from 'file-saver-es';
import { HttpClient } from '@angular/common/http';
import { StockService } from 'src/app/services/stock.service';

@Component({
  selector: 'app-inventario-final',
  templateUrl: './inventario-final.page.html',
  styleUrls: ['./inventario-final.page.scss'],
})
export class InventarioFinalPage implements OnInit {

  listaInventario: InventarioFinalDTO[] = [];
  listaInventarioCambios: InventarioFinalDTO[] = [];
  isSending = false;

  dataSource: DataSource = new DataSource(this.listaInventario);


  constructor(private ventaService: VentaService, private http: HttpClient, private stockService: StockService) { }

  ngOnInit() {

    this.ventaService.obtieneCajeroDeUnaFecha().then((productosService) => {
      productosService.subscribe((resul) => {
        this.listaInventario = resul.listEntities;
        console.log('lista inicial ', resul);

        this.listaInventario.forEach(x => {
          x.costoUnitario = 0.00;
          x.almacenCentral = 0.00;
          x.almacenOtroProveedor = 0.00;
          x.saldoTotal = 0.00;
          x.ingreso = 0.00;
          x.totalIngresos = 0.00;
          x.asignacionesBarra1 = 0.00;
          x.totalAsignacionesBarra1 = 0.00;
          x.asignacionesBarra2 = 0.00;
          x.totalAsignacionesBarra2 = 0.00;
          x.asignacionesBarra3 = 0.00;
          x.totalAsignacionesBarra3 = 0.00;
          x.devolucionesB1 = 0;
          x.devolucionesB2 = 0;
          x.devolucionesB3 = 0;
          x.totalDevoluciones = 0.00;
          x.precioVenta = 0.00;
          x.valorGanado = 0.00;
        });
        this.dataSource = resul.listEntities;
        this.ventaService.showMessageResponse(resul);
      });
    });

    // setInterval(() => {
    //   if (!this.isSending) {
    //     this.isSending = true;
    //     this.ventaService.grabarBufferInventario(this.listaInventario).then(x => {
    //       x.subscribe(resul => {
    //         console.log('resultado de grabar buffer', resul);
    //         if(resul.state === 1){
    //           console.log('se grabo correctamente', resul);
    //           this.isSending = false;
    //         }

    //       });
    //     });
    //   }
    // }, 10000);

  }

  clone(obj) {
    if (null == obj || 'object' != typeof obj) { return obj; }
    const copy = obj.constructor();
    for (const attr in obj) {
      if (obj.hasOwnProperty(attr)) { copy[attr] = obj[attr]; }
    }
    return copy;
  }


  rowUpdated(rowdata: any) {

    const inventario = this.listaInventario?.filter(z => z.idProducto === rowdata.data.idProducto)[0];

    const totalAsiganaciones = Number(inventario.totalAsignacionesBarra1) + Number(inventario.totalAsignacionesBarra2)
      + Number(inventario.totalAsignacionesBarra3);

    if (Number(inventario.ingreso) > 0) {
      inventario.totalIngresos = Number(inventario.totalIngresos) + Number(inventario.ingreso);
      this.agregarCambio(inventario);
      inventario.ingreso = 0;

    }
    else if (Number(inventario.asignacionesBarra1) > 0
      && Number(inventario.totalIngresos) > totalAsiganaciones) {
      inventario.totalAsignacionesBarra1 = Number(inventario.totalAsignacionesBarra1) + Number(inventario.asignacionesBarra1);
      this.agregarCambio(inventario);
      inventario.asignacionesBarra1 = 0;
    }
    else if (Number(inventario.asignacionesBarra2) > 0
      && Number(inventario.totalIngresos) > totalAsiganaciones) {
      inventario.totalAsignacionesBarra2 = Number(inventario.totalAsignacionesBarra2) + Number(inventario.asignacionesBarra2);
      this.agregarCambio(inventario);
      inventario.asignacionesBarra2 = 0;
    }
    else if (Number(inventario.asignacionesBarra3) > 0
      && Number(inventario.totalIngresos) > totalAsiganaciones) {
      inventario.totalAsignacionesBarra3 = Number(inventario.totalAsignacionesBarra3) + Number(inventario.asignacionesBarra3);
      this.agregarCambio(inventario);
      inventario.asignacionesBarra3 = 0;
    }
    else if (Number(inventario.devolucionesB1) > 0
      && Number(inventario.totalDevoluciones) + Number(inventario.devolucionesB1) <= Number(inventario.totalIngresos)) {
      inventario.totalDevoluciones = Number(inventario.totalDevoluciones) + Number(inventario.devolucionesB1);
      this.agregarCambio(inventario);
      inventario.devolucionesB1 = 0;
    }
    else if (Number(inventario.devolucionesB2) > 0
      && Number(inventario.totalDevoluciones) + Number(inventario.devolucionesB2) <= Number(inventario.totalIngresos)) {
      inventario.totalDevoluciones = Number(inventario.totalDevoluciones) + Number(inventario.devolucionesB2);
      this.agregarCambio(inventario);
      inventario.devolucionesB2 = 0;
    }
    else if (Number(inventario.devolucionesB3) > 0
      && Number(inventario.totalDevoluciones) + Number(inventario.devolucionesB3) <= Number(inventario.totalIngresos)) {
      inventario.totalDevoluciones = Number(inventario.totalDevoluciones) + Number(inventario.devolucionesB3);
      this.agregarCambio(inventario);
      inventario.devolucionesB3 = 0;
    }
    if (Number(inventario.costoUnitario) < 0) {
      inventario.costoUnitario = 0;
    }
    else {
      this.agregarCambio(inventario);
    }

    if (Number(inventario.precioVenta) < 0) {
      inventario.precioVenta = 0;
    }
    else {
      this.agregarCambio(inventario);
    }
  }

  agregarCambio(item: any) {
    item.valorGanado = (Number(item.precioVenta) - Number(item.costoUnitario)) *
      (Number(item.totalIngresos) - Number(item.totalDevoluciones));
    const clone = this.clone(item);
    clone.id = this.listaInventarioCambios.length;
    this.listaInventarioCambios.push(clone);
  }

  guardar() {
    this.stockService.grabarBufferInventario(this.listaInventarioCambios).then(resul => {
      resul.subscribe(resulService => {
        console.log('guardar registros', resulService);
        this.stockService.showMessageResponse(resulService);
        if (resulService.state === 1) {
          this.listaInventarioCambios = [];
        }
      });
    });

  }

}
