import { Component, Input, AfterViewInit } from '@angular/core';
import ArrayStore from 'devextreme/data/array_store';
import DataSource from 'devextreme/data/data_source';
import { IngredientesDeMenuGeneralDTO } from 'src/app/interfaces/inventario/IngredientesDeMenuGeneral';
import { InventarioService } from 'src/app/services/inventario.service';

@Component({
  selector: 'app-detalle-ingredientes',
  templateUrl: './detalle-ingredientes.component.html',
  styleUrls: ['./detalle-ingredientes.component.scss'],
})
export class DetalleIngredientesComponent implements AfterViewInit {
  @Input() key: number;
  @Input() listaDetalleIngredientes: IngredientesDeMenuGeneralDTO[];

  detalleDataSource: DataSource;
  //listaDetalleIngredientes: IngredientesDeMenuGeneralDTO[];

  constructor(private inventarioService: InventarioService) { 
   //ver como recibir info 
   //this.listaDetalleIngredientes = [];
  }
  ngAfterViewInit() {
    this.detalleDataSource = new DataSource({
      store: new ArrayStore({
        data: this.listaDetalleIngredientes,
        key: 'idIngrediente',
      }),
      filter: ['idPrecio', '=', this.key],
    });
  }


  completedValue(rowData) {
    return rowData.Status == 'Completed';
  }

}
