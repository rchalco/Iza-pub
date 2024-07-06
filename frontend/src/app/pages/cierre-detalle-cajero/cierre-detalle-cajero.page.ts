import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DataDocumento } from 'src/app/interfaces/general/documento';
import { DocumentoService } from 'src/app/services/documento.service';
import { VentaService } from 'src/app/services/venta.service';
import { customFormatter } from 'src/environments/environment';

@Component({
  selector: 'app-cierre-detalle-cajero',
  templateUrl: './cierre-detalle-cajero.page.html',
  styleUrls: ['./cierre-detalle-cajero.page.scss'],
})
export class CierreDetalleCajeroPage implements OnInit {
  listReporteCierre = [];
  total = 0.0;
  totalEfec = 0.0;
  totalPOS = 0.0;
  totalTicket = 0.0;
  totalDiferencia = 0.0;

  constructor(
    private ventaService: VentaService,
    private documentoService: DocumentoService
  ) { }

  ngOnInit() {
    this.cargarCierreTotal();
  }

  cargarCierreTotal() {
    this.total = 0;
    this.totalEfec = 0;
    this.totalDiferencia = 0;
    this.ventaService.obtieneReporteCierreGlobalCajero().then((service) => {
      service.subscribe((response) => {
        if (response.state !== 1) {
          this.ventaService.showMessageResponse(response);
        }
        console.log('response', response);
        this.listReporteCierre = response.listEntities;
        this.listReporteCierre.forEach((x) => {
          this.total += x.montoDeclarado;
          this.totalDiferencia += x.diferencia;
          this.totalEfec += x.monto;
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
    doc.titulosTabla.push('Cajero');
    doc.titulosTabla.push('Forma Pago');
    doc.titulosTabla.push('Monto');
    doc.titulosTabla.push('Declarado');
    doc.titulosTabla.push('Diferencia');

    doc.contenido = new Array();
    this.listReporteCierre.forEach((x) => {
      doc.contenido.push(
        x.descripcion +
        '|' +
        x.nombre +
        '|' +
        x.formaDePago +
        '|' +
        x.monto +
        '|' +
        x.montoDeclarado +
        '|' +
        x.diferencia
      );
    });

    doc.contenido.push(
      '| ' +
      '| ' +
      '| ' +
      this.totalEfec +
      '| ' +
      this.total +
      '| ' +
      this.totalDiferencia
    );
    doc.pie =
      // eslint-disable-next-line max-len
      '\n\n\n\nFirma Responsable 1                                   Firma Responsable 2 \n\nReporte Generado para el SISTEMA FABULA, para cualquier consulta escribir a contactos@gamatek.org';

    console.log('Documento', doc);

    this.documentoService.generarDocumentoMultiPlataforma(doc);
  }
}
