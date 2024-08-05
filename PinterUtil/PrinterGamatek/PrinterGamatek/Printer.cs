using Microsoft.PointOfService;
using PdfiumViewer;
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
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using jp.co.epson.uposcommon;

namespace PrinterGamatek
{
    public partial class Printer : Form
    {
        public List<PrinterLineResponse> printerLineResponses { get; set; }
        PosPrinter m_Printer = null;

        public Printer()
        {
            InitializeComponent();
        }

        private void Printer_Load(object sender, EventArgs e)
        {
            InitPrinter();
            //try
            //{
            //    //As using the PrintNormal method, send strings to a printer, and print it
            //    //[\n] is the standard code for starting a new line.
            //    m_Printer.PrintNormal(PrinterStation.Receipt, "Hola mundo .Net\n");
            //}
            //catch (PosControlException ex)
            //{
            //    MessageBox.Show($"Imposible imprimir texto {ex.Message}");
            //}
        }

        private void timerPrinter_Tick(object sender, EventArgs e)
        {
            timerPrinter.Stop();
            timerPrinter.Enabled = false;
            Debug.Print("timer activo");
            obtenerDocumentosPendientes();
            timerPrinter.Enabled = true;
            timerPrinter.Start();
        }

        private async void obtenerDocumentosPendientes()
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

            //Thread.Sleep(1000);
            ///Iniciamos la impresion
            string namePrinter = ConfigurationManager.AppSettings["namePrinter"];

            for (int i = 0; i < printerLineResponses.Count; i++)
            {
                string filename = Guid.NewGuid().ToString();
                File.WriteAllBytes(filename, Convert.FromBase64String(printerLineResponses[i].documentB64));
                ///TODO aca debo imprimir
                ///PrintPDF(namePrinter, filename);
                //Thread.Sleep(1000);
                //lstDocument.Items.Remove(printerLineResponses[i].nombreDocumento);
                File.Delete(filename);
            }

        }

        public bool PrintPDF(string printer, string filename)
        {
            try
            {
                // Create the printer settings for our printer
                var printerSettings = new PrinterSettings
                {
                    PrinterName = printer,
                    Copies = 1,
                };

                // Create our page settings for the paper size selected
                var pageSettings = new PageSettings(printerSettings)
                {
                    Margins = new Margins(0, 0, 0, 0),
                };

                // Now print the PDF document
                using (var document = PdfDocument.Load(filename))
                {
                    using (var printDocument = document.CreatePrintDocument())
                    {
                        printDocument.PrinterSettings = printerSettings;
                        printDocument.DefaultPageSettings = pageSettings;
                        printDocument.PrintController = new StandardPrintController();
                        printDocument.Print();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void InitPrinter()
        {
            //<<<step1>>>--Start
            //Use a Logical Device Name which has been set on the SetupPOS.
            string strLogicalName = "PosPrinter";
            try
            {
                //Create PosExplorer
                PosExplorer posExplorer = new PosExplorer();

                DeviceInfo deviceInfo = null;

                try
                {
                    deviceInfo = posExplorer.GetDevice(DeviceType.PosPrinter, strLogicalName);
                    m_Printer = (PosPrinter)posExplorer.CreateInstance(deviceInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Imposible inicar la impresora {ex.Message}");
                    return;
                }

                //Open the device
                m_Printer.Open();

                //Get the exclusive control right for the opened device.
                //Then the device is disable from other application.
                m_Printer.Claim(1000);

                //Enable the device.
                m_Printer.DeviceEnabled = true;
            }
            catch (PosControlException ex)
            {
                MessageBox.Show($"Imposible inicar la impresora {ex.Message}");
            }
            //<<<step1>>>--End

        }
    }


}
