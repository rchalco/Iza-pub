import { Component, OnInit } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import DxDataGrid from 'devextreme/ui/data_grid';
import { exportDataGrid } from 'devextreme/excel_exporter';
import { saveAs } from 'file-saver-es';
import { ProductoReposicion } from 'src/app/interfaces/inventario/ProductoReposicion';
import { StockService } from 'src/app/services/stock.service';
import { Workbook } from 'exceljs';

@Component({
  selector: 'app-reposicion-producto2',
  templateUrl: './reposicion-producto2.page.html',
  styleUrls: ['./reposicion-producto2.page.scss'],
})
export class ReposicionProducto2Page implements OnInit {

  listaProductos: ProductoReposicion[] = [];
  dataSource: DataSource = new DataSource(this.listaProductos);


  constructor(private stockService: StockService) { }

  ngOnInit() {
    this.cargaDatos();

  }
  cargaDatos() {

    this.stockService.ObtieneMovimientoAlmacenCentral().then((resulPromise) => {
      resulPromise.subscribe((resul) => {
        console.log(resul);
        const vlistaProductos = resul.listEntities;
        this.listaProductos = vlistaProductos;
        this.dataSource = vlistaProductos;
      });
    });
  }
  calculaCellTotal(data: any) {
    return data.cantidadActual + data.cantidadAdicional;
  }


  onSaving(e: any) {
    e.cancel = true;
    console.log('cambios', e);
    console.log('cambios1', JSON.stringify(e.changes));

    if (e.changes.length) {
      e.promise = this.processBatchRequest(`${URL}/Batch`, JSON.stringify(e.changes), e.component);

    }
  }

  async processBatchRequest(url: string, changes: string, component: DxDataGrid): Promise<any> {

    this.stockService.grabaReposicionProductosJSON(changes);
    this.cargaDatos();
    await component.refresh(true);
    component.cancelEditData();
  }

  onExporting(e: any) {

    const workbook = new Workbook();
    const worksheet = workbook.addWorksheet('Employees');

    exportDataGrid({
      component: e.component,
      worksheet,
      autoFilterEnabled: true,
    }).then(() => {
      workbook.xlsx.writeBuffer().then((buffer) => {
        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'DataGrid.xlsx');
      });
    });
    e.cancel = true;
  }




}
