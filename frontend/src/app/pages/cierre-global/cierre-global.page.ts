import { Component, OnInit } from '@angular/core';
import { DataDocumento } from 'src/app/interfaces/general/documento';
import { DocumentoService } from 'src/app/services/documento.service';
import { VentaService } from 'src/app/services/venta.service';

@Component({
  selector: 'app-cierre-global',
  templateUrl: './cierre-global.page.html',
  styleUrls: ['./cierre-global.page.scss'],
})
export class CierreGlobalPage implements OnInit {
  listReporteCierre = [];
  total = 0.0;
  totalEfec = 0.0;
  totalPOS = 0.0;
  totalTicket = 0.0;

  constructor(
    private ventaService: VentaService,
    private documentoService: DocumentoService
  ) {}

  ngOnInit() {
    this.cargarCierreTotal();
  }

  cargarCierreTotal() {
    this.ventaService.obtieneReporteCierreTotal().then((service) => {
      service.subscribe((response) => {
        if (response.state !== 1) {
          this.ventaService.showMessageResponse(response);
        }
        console.log('response', response);
        this.listReporteCierre = response.listEntities;
        this.listReporteCierre.forEach((x) => {
          this.total += x.total;
          this.totalEfec += x.efectivo;
          this.totalPOS += x.pos;
          this.totalTicket += x.ticket;
        });
      });
    });
  }

  imprimir() {
    console.log('imprimir Cierre Total init');
    const doc = new DataDocumento();
    doc.isTableDocument = true;
    doc.titulo = 'CIERRE DIARIO FABULA ' + new Date().toLocaleDateString();
    doc.titulo = doc.titulo + '\n' + '(Expresado en Bs.)';

    doc.titulosTabla = new Array();
    doc.titulosTabla.push('Barra');
    doc.titulosTabla.push('Efectivo');
    doc.titulosTabla.push('POS');
    doc.titulosTabla.push('Ticket');
    doc.titulosTabla.push('Total');

    doc.contenido = new Array();
    this.listReporteCierre.forEach((x) => {
      doc.contenido.push(
        x.descripcion +
          '|' +
          x.efectivo +
          '|' +
          x.pos +
          '|' +
          x.ticket +
          '|' +
          x.total
      );
    });

    doc.contenido.push(
      '' +
        '|' +
        this.totalEfec +
        '|' +
        this.totalPOS +
        '|' +
        this.totalTicket +
        '|' +
        this.total
    );
    doc.pie =
      'Reporte Generado para el SISTEMA VENTAS, para cualquier consulta escribir a contactos@gamatek.org';

    console.log('Documento', doc);

    this.documentoService.generarDocumentoMultiPlataforma(doc);
  }
}
