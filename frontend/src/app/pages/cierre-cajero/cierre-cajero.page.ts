import { environment } from './../../../environments/environment.prod';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DataDocumento } from 'src/app/interfaces/general/documento';
import { DocumentoService } from 'src/app/services/documento.service';
import { VentaService } from 'src/app/services/venta.service';
import { customFormatter } from 'src/environments/environment';

@Component({
  selector: 'app-cierre-cajero',
  templateUrl: './cierre-cajero.page.html',
  styleUrls: ['./cierre-cajero.page.scss'],
})
export class CierreCajeroPage implements OnInit {
  formasPagos = [];
  formasPagosTotales = [];
  montoApertura = 0;
  montoTotalVenta = 0;
  cajeros = [];
  currentCajero;
  observaciones = '';

  constructor(
    private ventaService: VentaService,
    private documentoService: DocumentoService,
    private router: Router
  ) { }

  ngOnInit() {
    this.obtieneCajeros();
  }

  obtieneCajeros() {

    
    this.ventaService.obtenerCajeros().then((service) => {
      service.subscribe((resul) => {
        console.log('obtenerCajeros', resul);
        this.cajeros = resul.listEntities;
        this.montoTotalVenta = 0;
        this.montoApertura = 0;
        this.formasPagosTotales = [];
        this.currentCajero = null;
      });
    });
  }

  obtieneArqueo(event) {
    if (!event.detail.value) {
      return;
    }
    this.montoTotalVenta = 0;
    this.montoApertura = 0;
    this.formasPagosTotales = [];
    console.log('obtieneArqueo', event);
    this.currentCajero = event.detail.value;
    this.ventaService
      .obtenerArqueoCajero(this.currentCajero.idOperacionDiariaCaja)
      .then((service) => {
        service.subscribe(async (resul) => {
          this.formasPagos = resul.listEntities;
          console.log('this.formasPagos', this.formasPagos);
          ///TODO llenamos las formas de pagos totales
          this.formasPagos.forEach((x) => {
            this.montoApertura = x.montoApertura;
            if (
              this.formasPagosTotales.filter(
                (yy) => yy.idFormaDePago === x.idFormaDePago
              ).length === 0
            ) {
              const totalFormaPago = {
                idFormaDePago: x.idFormaDePago,
                formaDePago: x.formaDePago,
                totalVendido: 0,
                diferencia: 0,
                totalDeclarado: 0,
                observaciones: ''
              };
              totalFormaPago.totalVendido = this.formasPagos
                .filter((zzz) => zzz.idFormaDePago === x.idFormaDePago)
                .reduce((sum, current) => sum + current.montoCubierto, 0);
              totalFormaPago.observaciones = this.observaciones;
              this.formasPagosTotales.push(totalFormaPago);
              this.montoTotalVenta += totalFormaPago.totalVendido;
            }
          });
        });
      });
  }

  realizarCierre() {
    this.formasPagosTotales.forEach(x => {
      x.observaciones = this.observaciones;
    });

    this.ventaService
      .realizarCierreCaja(
        this.formasPagosTotales,
        this.montoApertura,
        this.montoTotalVenta,
        this.currentCajero.idOperacionDiariaCaja
      )
      .then((services) => {
        services.subscribe((resul) => {
          this.ventaService.showMessageResponse(resul);
          this.obtieneCajeros();
          this.imprimirCierre();
        });
      });
  }

  calcularDiferencia(pFormaPago) {
    pFormaPago.diferencia = pFormaPago.totalVendido - pFormaPago.entregado;
    pFormaPago.totalDeclarado = pFormaPago.entregado;
  }

  imprimirCierre() {
    console.log('imprimirCierre init');
    const doc = new DataDocumento();
    doc.titulo = '-----------------------------------';
    doc.titulo = doc.titulo + '\n' + 'CIERRE CAJERO';
    //doc.titulo = doc.titulo + '\n' + this.empleadoSeleccionado.nombreCompleto;
    //doc.titulo = doc.titulo + '\n' + this.selectedLugarConsumo.descripcion;
    doc.titulo = doc.titulo + '\n' + '-----------------------------------';

    doc.contenido = new Array();
    doc.contenido.push('Monto Apertura:...Bs.' + this.montoApertura);
    doc.contenido.push('Total Ventas:.....Bs.' + this.montoTotalVenta);
    doc.contenido.push(
      'Datos Cajero: ' +
      this.currentCajero.ci +
      ' ' +
      this.currentCajero.nombreCompleto
    );
    doc.contenido.push('Barra: ' + this.currentCajero.barra);
    doc.contenido.push('Observaciones: ' + this.observaciones);
    doc.contenido.push('=============================');
    doc.contenido.push('...Totales por metodo de pago...');
    doc.contenido.push('=============================');

    this.formasPagosTotales.forEach((x) => {
      x.diferencia = x.diferencia ? x.diferencia : 0;
      x.entregado = x.entregado ? x.entregado : 0;
      doc.contenido.push('Forma de Pago: .....' + x.formaDePago);
      doc.contenido.push('Total:..........' + customFormatter(x.totalVendido));
      doc.contenido.push('Entregado:......' + customFormatter(x.entregado));
      doc.contenido.push('Diferencia:.....' + customFormatter(x.diferencia));
      doc.contenido.push('*************************\n');
    });

    doc.pie = '\n\n\n\nFirma Cajero';
    doc.pie +=
      '\nCI: ' +
      this.currentCajero.ci +
      '\nNombre: ' +
      this.currentCajero.nombreCompleto;
    doc.pie += '\n\n\n\nFirma Administrador';
    console.log('Documento', doc);

    this.documentoService.generarDocumentoMultiPlataforma(doc);
  }
}
