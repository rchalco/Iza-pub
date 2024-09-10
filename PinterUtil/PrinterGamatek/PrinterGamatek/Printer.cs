using DevExpress.Pdf;
//using Microsoft.PointOfService;
using PrinterGamatek.CleintService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Printing;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;


namespace PrinterGamatek
{
    public partial class Printer : Form
    {
        public List<PrinterLineResponse> printerLineResponses { get; set; }
        //PosPrinter m_Printer = null;

        public Printer()
        {
            InitializeComponent();
        }

        private void Printer_Load(object sender, EventArgs e)
        {

        }

        private async void timerPrinter_Tick(object sender, EventArgs e)
        {
            timerPrinter.Stop();
            timerPrinter.Enabled = false;
            Debug.Print("timer activo");
            await obtenerDocumentosPendientes();
            string namePrinter = ConfigurationManager.AppSettings["namePrinter"];
            int timeFoWait = Convert.ToInt32(ConfigurationManager.AppSettings["timeFoWait"]);
            int cont = 0;
            await Task.Delay(1000);
            while (lstDocument.Items.Count > 0)
            {
                lstDocument.Items.RemoveAt(0);
                //imprimirDocumento(printerLineResponses[cont].documentB64, namePrinter);
                imprimirDocumentoPDF(printerLineResponses[cont].documentB64, namePrinter);                
                cont++;                
                await Task.Delay((int)timeFoWait);
            }            
            timerPrinter.Enabled = true;
            timerPrinter.Start();
        }
        private async Task obtenerDocumentosPendientes()
        {
            ClientHelper clientHelper = new ClientHelper();
            string urlService = ConfigurationManager.AppSettings["urlPrinterLine"];
            int idPrinter = Convert.ToInt32(ConfigurationManager.AppSettings["idPrinter"]);
            PrinterLineRequest printerLineRequest = new PrinterLineRequest
            {
                PrinterId = idPrinter,
            };
            var responseDocuments = await clientHelper.Consume<ResponseQuery<PrinterLineResponse>>(urlService, printerLineRequest);
            if (responseDocuments.State != ResponseType.Success)
            {
                MessageBox.Show(responseDocuments.Message);
                return;
            }
            printerLineResponses = responseDocuments.ListEntities;

            printerLineResponses.ForEach(printerLineResponse =>
            {
                lstDocument.Items.Add(printerLineResponse.nombreDocumento);
            });
        }

        private void imprimirDocumento(string documentB64, string namePrinter)
        {
            string filename = Guid.NewGuid().ToString() + ".pdf";
            File.WriteAllBytes(filename, Convert.FromBase64String(documentB64));
            string fullFilePathForPrintProcess = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), filename);
            ///TODO aca debo imprimir
            PrintUsingAdobeAcrobat(fullFilePathForPrintProcess, namePrinter);
        }

        private void imprimirDocumentoPDF(string documentB64, string namePrinter)
        {
            try
            {
                var settings = new PdfPrinterSettings();
                //settings.Settings.PrinterName = "Canon G3060 series";
                settings.Settings.PrinterName = namePrinter;
                settings.Settings.PrintToFile = true;
                settings.PageOrientation = PdfPrintPageOrientation.Portrait;
                settings.ScaleMode = PdfPrintScaleMode.ActualSize;

                string filename = Guid.NewGuid().ToString() + ".pdf";
                File.WriteAllBytes(filename, Convert.FromBase64String(documentB64));
                string fullFilePathForPrintProcess = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), filename);


                //var pdfFileName = "Demo.pdf";
                var pdfViewer2 = new DevExpress.Xpf.PdfViewer.PdfViewerControl();
                pdfViewer2.AsyncDocumentLoad = false;
                pdfViewer2.OpenDocument(fullFilePathForPrintProcess);
                pdfViewer2.ShowOpenFileOnStartScreen = false;
                pdfViewer2.Print(settings);
                if (pdfViewer2.CloseDocumentCommand != null)
                {
                    pdfViewer2.CloseDocumentCommand.Execute(null);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            

        }

        public static void PrintUsingAdobeAcrobat(string fullFilePathForPrintProcess, string printerName)
        {
            string printApplicationPath = Microsoft.Win32.Registry.LocalMachine
            .OpenSubKey("Software")
            .OpenSubKey("Microsoft")
            .OpenSubKey("Windows")
            .OpenSubKey("CurrentVersion")
            .OpenSubKey("App Paths")
            .OpenSubKey("Acrobat.exe")
            .GetValue(String.Empty).ToString();

            const string flagNoSplashScreen = "/s";
            const string flagOpenMinimized = "/h";

            var flagPrintFileToPrinter = string.Format("/t \"{0}\" \"{1}\"", fullFilePathForPrintProcess, printerName);

            var args = string.Format("{0} {1} {2}", flagNoSplashScreen, flagOpenMinimized, flagPrintFileToPrinter);

            var startInfo = new ProcessStartInfo
            {
                FileName = printApplicationPath,
                Arguments = args,
                CreateNoWindow = true,
                ErrorDialog = false,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            var process = Process.Start(startInfo);
            process.EnableRaisingEvents = true;
            //process.WaitForExit();
            process?.Dispose();
        }

        public static void PrintUsingCmd(string fullFilePathForPrintProcess, string printerName)
        {
            string fileCommand = "powershell.exe";
            string cmdPrint = $"Start-Process -FilePath \"{fullFilePathForPrintProcess}\" -Verb Print -ArgumentList \"{printerName}\" -PassThru | kill";
            var startInfo = new ProcessStartInfo()
            {
                FileName = fileCommand,
                Arguments = cmdPrint,
                UseShellExecute = false
            };
            var process = Process.Start(fileCommand, cmdPrint);
            process.EnableRaisingEvents = true;
            process.WaitForExit();
            process?.Dispose();
        }
    }

}



