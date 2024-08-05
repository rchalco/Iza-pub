namespace PrinterGamatek
{
    partial class Printer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.printDialogMain = new System.Windows.Forms.PrintDialog();
            this.lstDocument = new System.Windows.Forms.ListBox();
            this.timerPrinter = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // printDialogMain
            // 
            this.printDialogMain.UseEXDialog = true;
            // 
            // lstDocument
            // 
            this.lstDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDocument.FormattingEnabled = true;
            this.lstDocument.Location = new System.Drawing.Point(0, 0);
            this.lstDocument.Name = "lstDocument";
            this.lstDocument.Size = new System.Drawing.Size(800, 450);
            this.lstDocument.TabIndex = 0;
            // 
            // timerPrinter
            // 
            this.timerPrinter.Enabled = true;
            this.timerPrinter.Interval = 1000;
            this.timerPrinter.Tick += new System.EventHandler(this.timerPrinter_Tick);
            // 
            // Printer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lstDocument);
            this.Name = "Printer";
            this.Text = "Cola de impresion";
            this.Load += new System.EventHandler(this.Printer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PrintDialog printDialogMain;
        private System.Windows.Forms.ListBox lstDocument;
        private System.Windows.Forms.Timer timerPrinter;
    }
}