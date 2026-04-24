import { FileOpener } from '@capacitor-community/file-opener';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BaseService } from './baseService';
import { LoadingController, Platform, ToastController } from '@ionic/angular';
import {
  environment,
  HEADERS_SERVICE,
  URL_MIROVENTA,
  URL_TINTORERIA,
} from 'src/environments/environment';
import { Observable, throwError } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { DatabaseService } from './DatabaseService';
import { URL_MIROVENTAOPERACION } from './../../environments/environment';
import { Filesystem, Directory, Encoding } from '@capacitor/filesystem';
import { PrinterHelper } from 'src/app/helpers/printer.helper';

const urlMicroventa = URL_MIROVENTAOPERACION;
const urlTintoreria = URL_MIROVENTA;
const headers = HEADERS_SERVICE;

@Injectable({
  providedIn: 'root',
})
export class DocumentoService extends BaseService {
  constructor(
    public databaseService: DatabaseService,
    public httpClient: HttpClient,
    public loadingController: LoadingController,
    public toastController: ToastController,
    public platform: Platform,
    private router: Router,
  ) {
    super(databaseService, httpClient, loadingController, toastController);
  }

  async generarDocumentoMultiPlataforma(dataDocumento: any) {



    await this.platform.ready().then((resul) => {
      console.log('se cargo la plataforma correctamente');
    });
    if (this.platform.is('android') || this.platform.is('ios')) {
      console.log('generarDocumentoMultiPlataforma android');
      this.generarDocumentoPartial(dataDocumento).subscribe(async (resul) => {
        console.log('generarDocumentoPartial terminado');
        const base64String = btoa(
          String.fromCharCode(...new Uint8Array(resul))
        );

        await this.imprimirDocumentoMovil(base64String);
      });
    } else {
      this.generarDocumento(dataDocumento);
    }
  }

  private async imprimirDocumentoMovil(base64: string): Promise<void> {
    const preferredPrinter = PrinterHelper.getPreferredPrinter().address;

    if (!preferredPrinter) {
      PrinterHelper.savePendingSalePrint(base64);
      await this.showMessageWarning(
        'Configura una impresora para continuar con la impresion.',
      );
      this.router.navigate(['/config-printer']);
      return;
    }

    try {
      await PrinterHelper.printPdfBase64(base64, preferredPrinter);
    } catch (error: unknown) {
      console.error('Error al imprimir documento:', error);
      PrinterHelper.savePendingSalePrint(base64);
      await this.showMessageWarning(
        'No se pudo imprimir. Verifica la impresora y vuelve a configurar.',
      );
      this.router.navigate(['/config-printer']);
    }
  }

  generarDocumento(dataDocumento: any) {

    console.log('ZZZZZZZZ',dataDocumento);
    let url_query = urlTintoreria + 'GenerarDocumento';
    this.presentLoader();
    this.httpClient
      .post(url_query, JSON.stringify(dataDocumento), {
        responseType: 'arraybuffer',
        headers,
      })
      .pipe(
        finalize(() => {
          console.log('se termino la llamada GenerarDocumento');
          this.dismissLoader();
        }),
        catchError((error) => {
          this.showMessageError('No se tiene comunicacion con el servidor');
          return throwError(() => new Error(error.status));
        })
      )
      .subscribe((x) => {
        this.downLoadFile(x, 'pdf');
      });
  }
  downLoadFile(data: any, type: string) {
    const blob = new Blob([data], { type: 'application/pdf' });
    const url = window.URL.createObjectURL(blob);
    const pwa = window.open(url);
    if (!pwa || pwa.closed || typeof pwa.closed == 'undefined') {
      alert(
        'Por favor deshabilite los bloqueadores de descarga para continuar.'
      );
    }
  }

  generarDocumentoPartial(dataDocumento: any) {


    this.presentLoader();
    return this.httpClient
      .post(urlTintoreria + 'GenerarDocumento', JSON.stringify(dataDocumento), {
        responseType: 'arraybuffer',
        headers,
      })
      .pipe(
        finalize(() => {
          console.log('se termino la llamada GenerarDocumento');
          this.dismissLoader();
        }),
        catchError((error) => {
          this.showMessageError('No se tiene comunicacion con el servidor');
          console.error(
            'error en generarDocumentoPartial',
            JSON.stringify(error)
          );
          return throwError(() => new Error(error.status));
        })
      );
  }

  ///TODO generador de documentos PFD para Android
  writeFilePDF = async (fileName, dataToWrite) => {
    console.log('writeFilePDF');
    await Filesystem.writeFile({
      path: fileName,
      data: dataToWrite,
      directory: Directory.Documents,
      recursive: true,
    }).then(
      (writeFileResponse) => {
        console.log(
          'archivo escrito correctamente => ',
          JSON.stringify(writeFileResponse)
        );
      },
      (error) => {
        console.error('error al escribir del archivo => ', error);
      }
    );
  };

  ///TODO abrir un archivo
  openFilePDF = async (fileName) => {
    // Browser.open({
    //   url: Directory.Documents + fileName,
    // });

    const fileFullName =
      'file:///storage/emulated/0/' + Directory.Documents + '/' + fileName;
    FileOpener.open({
      filePath: fileFullName,
      contentType: 'application/pdf',
    })
      .then(() => {
        console.log('archivo abierto', fileFullName);
      })
      .catch((e) => {
        console.error(
          'error al abrir el archivo ' + fileFullName,
          JSON.stringify(e)
        );
      });
  };

  readSecretFile = async (filePath) => {
    const contents = await Filesystem.readFile({
      path: filePath,
      directory: Directory.Documents,
      encoding: Encoding.UTF8,
    });

    console.log('secrets:', contents);
  };

  deleteSecretFile = async (filePath) => {
    await Filesystem.deleteFile({
      path: filePath,
      directory: Directory.Documents,
    });
  };

  readFilePath = async (filePath) => {
    const contents = await Filesystem.readFile({
      path: filePath,
    });

    console.log('data:', contents);
  };
}
