import {
  BluetoothDevice,
  CapacitorThermalPrinter,
} from 'capacitor-thermal-printer';

export interface StoredPrinterConfig {
  address: string | null;
  name: string | null;
}

export class PrinterHelper {
  static readonly PRINTER_STORAGE_KEY = 'preferredPrinter';
  static readonly PRINTER_NAME_STORAGE_KEY = 'preferredPrinterName';
  static readonly PENDING_PDF_PRINT_KEY = 'pendingSalePdfPrint';

  static getPreferredPrinter(): StoredPrinterConfig {
    return {
      address: localStorage.getItem(PrinterHelper.PRINTER_STORAGE_KEY),
      name: localStorage.getItem(PrinterHelper.PRINTER_NAME_STORAGE_KEY),
    };
  }

  static savePreferredPrinter(device: Pick<BluetoothDevice, 'address' | 'name'>): void {
    localStorage.setItem(PrinterHelper.PRINTER_STORAGE_KEY, device.address);

    if (device.name) {
      localStorage.setItem(PrinterHelper.PRINTER_NAME_STORAGE_KEY, device.name);
    } else {
      localStorage.removeItem(PrinterHelper.PRINTER_NAME_STORAGE_KEY);
    }
  }

  static getPendingSalePrint(): string | null {
    return localStorage.getItem(PrinterHelper.PENDING_PDF_PRINT_KEY);
  }

  static savePendingSalePrint(base64: string): void {
    localStorage.setItem(PrinterHelper.PENDING_PDF_PRINT_KEY, base64);
  }

  static clearPendingSalePrint(): void {
    localStorage.removeItem(PrinterHelper.PENDING_PDF_PRINT_KEY);
  }

  static isAlreadyConnectedError(error: unknown): boolean {
    const message = PrinterHelper.getErrorMessage(error).toLowerCase();
    return (
      message.includes('already') &&
      (message.includes('connected') || message.includes('connect'))
    );
  }

  static isNotConnectedError(error: unknown): boolean {
    const message = PrinterHelper.getErrorMessage(error).toLowerCase();
    return (
      message.includes('not connected') ||
      message.includes('printer is not connected')
    );
  }

  static async connectToPrinter(address: string): Promise<void> {
    try {
      await CapacitorThermalPrinter.connect({ address });
      return;
    } catch (error: unknown) {
      if (PrinterHelper.isAlreadyConnectedError(error)) {
        return;
      }
    }

    await CapacitorThermalPrinter.disconnect().catch(() => undefined);
    await CapacitorThermalPrinter.connect({ address });
  }

  static async reconnectToPrinter(address: string): Promise<void> {
    await CapacitorThermalPrinter.disconnect().catch(() => undefined);
    await CapacitorThermalPrinter.connect({ address });
  }

  static async printPdfBase64(base64: string, printerAddress: string): Promise<void> {
    const pdfjsLib = await import('pdfjs-dist');
    pdfjsLib.GlobalWorkerOptions.workerSrc = 'pdf.worker.min.mjs';

    const pdf = await pdfjsLib.getDocument({
      data: PrinterHelper.base64ToUint8Array(base64),
    }).promise;

    const PRINTER_WIDTH_MM = 72;
    const PRINTER_DPI = 203;
    const PRINTER_WIDTH_PX = Math.round((PRINTER_WIDTH_MM * PRINTER_DPI) / 25.4);

    const blobs: Blob[] = [];
    for (let pageNum = 1; pageNum <= pdf.numPages; pageNum++) {
      const page = await pdf.getPage(pageNum);
      const naturalViewport = page.getViewport({ scale: 1 });
      const scale = PRINTER_WIDTH_PX / naturalViewport.width;
      const viewport = page.getViewport({ scale });
      const canvas = document.createElement('canvas');
      canvas.width = PRINTER_WIDTH_PX;
      canvas.height = Math.round(viewport.height);
      const ctx = canvas.getContext('2d');

      if (!ctx) {
        throw new Error('No se pudo inicializar el canvas para impresion.');
      }

      await page.render({ canvasContext: ctx as any, viewport } as any).promise;
      const blob = await new Promise<Blob>((resolve, reject) => {
        canvas.toBlob(
          (value) => (value ? resolve(value) : reject(new Error('Canvas toBlob failed'))),
          'image/png',
        );
      });
      blobs.push(blob);
    }

    await PrinterHelper.connectToPrinter(printerAddress);

    try {
      await PrinterHelper.writeImageBlobs(blobs, PRINTER_WIDTH_MM);
    } catch (error: unknown) {
      if (!PrinterHelper.isNotConnectedError(error)) {
        throw error;
      }

      await PrinterHelper.reconnectToPrinter(printerAddress);
      await PrinterHelper.writeImageBlobs(blobs, PRINTER_WIDTH_MM);
    }
  }

  static getErrorMessage(error: unknown): string {
    return error instanceof Error ? error.message : String(error);
  }

  static base64ToUint8Array(base64: string): Uint8Array {
    const cleanBase64 = base64.includes(',') ? base64.split(',')[1] : base64;
    const binaryData = atob(cleanBase64);
    const byteArray = new Uint8Array(binaryData.length);

    for (let i = 0; i < binaryData.length; i++) {
      byteArray[i] = binaryData.charCodeAt(i);
    }

    return byteArray;
  }

  private static async writeImageBlobs(
    blobs: Blob[],
    printerWidthMm: number,
  ): Promise<void> {
    CapacitorThermalPrinter.begin().limitWidth(printerWidthMm);

    for (let i = 0; i < blobs.length; i++) {
      CapacitorThermalPrinter.image(blobs[i]);
      if (i < blobs.length - 1) {
        CapacitorThermalPrinter.feedCutPaper();
      }
    }

    await CapacitorThermalPrinter.write();
  }
}
