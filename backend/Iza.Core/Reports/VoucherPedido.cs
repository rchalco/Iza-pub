using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Iza.Core.Reports
{
    public partial class VoucherPedido : DevExpress.XtraReports.UI.XtraReport
    {
        public VoucherPedido()
        {
            
            InitializeComponent();

            ReportUnit = ReportUnit.TenthsOfAMillimeter;

            PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
            Landscape = false;
            RollPaper = true;

            PageWidth = 800;  // 80mm
            PageHeight = 2000; // alto base para PDF

            Margins = new DevExpress.Drawing.DXMargins(0, 0, 5, 5);


            // Unidad en décimas de milímetro
            //this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;

            //// Impresora térmica → ancho 80mm = 800 décimas de mm
            //this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
            //this.PageWidth = 620;     // 80 mm exactos
            //this.PageHeight = 1200;   // Se ignora en RollPaper, pero evita errores

            //// Impresión de rollo (continuo)
            //this.RollPaper = true;

            //// Márgenes mínimos (térmicas no usan margen)
            //this.Margins.Top = 5;
            //this.Margins.Bottom = 5;
            //this.Margins.Left = 0;
            //this.Margins.Right = 0;

            //// Sigue siendo vertical
            //this.Landscape = false;
        }
    }
}
