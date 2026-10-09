using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using System.Globalization;


using Iza.Core.Base;
using Iza.Core.Domain.General;
using PlumbingProps.Wrapper;


namespace Iza.Core.Engine.Impresion
{
    public class EngineImpresion : BaseManager
    {
        public Response GenerarDocumento(DataDocumento dataDocumento)
        {
            Response response = new Response();
            try
            {
                Table table = new Table(1, false);
                table.SetMinWidth(290f);
                table.SetMaxWidth(290f);
                table.SetMargins(0, 0, 0, 0);
                table.SetBorder(Border.NO_BORDER);

                string pathLogo = string.IsNullOrEmpty(dataDocumento.pathLogo) ? @"c:\fonts\Logo.jpg" : dataDocumento.pathLogo;
                Image img = new Image(ImageDataFactory
               .Create(pathLogo))
               .SetHeight(60)
               .SetWidth(150)
               .SetTextAlignment(TextAlignment.CENTER);
                Cell cellContent = new Cell(1, 1);
                cellContent.Add(img);
                cellContent.SetBorder(Border.NO_BORDER);
                table.AddCell(cellContent);

                FontProgram fontProgram = FontProgramFactory.CreateFont(@"c:\fonts\arial.ttf");
                PdfFont fuenteExterna = PdfFontFactory.CreateFont(fontProgram, PdfEncodings.WINANSI);

                Text text = new Text(dataDocumento.titulo)
                    .SetFont(fuenteExterna)
                    .SetBold();

                Paragraph subheader = new Paragraph(text)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(14);
                cellContent = new Cell(2, 1);
                cellContent.Add(subheader);
                cellContent.SetBorder(Border.NO_BORDER);
                table.AddCell(cellContent);

                LineSeparator lineSeparator = new LineSeparator(new SolidLine());
                cellContent = new Cell(3, 1);
                cellContent.Add(lineSeparator);
                cellContent.SetBorder(Border.NO_BORDER);
                table.AddCell(cellContent);

                #region  para hacer tablas
                //// Table
                //Table table = new Table(2, false);
                //table.SetWidth(520);
                //table.SetTextAlignment(TextAlignment.CENTER);
                //Cell cell11 = new Cell(1, 1)
                //   .SetBackgroundColor(ColorConstants.GRAY)
                //   .SetTextAlignment(TextAlignment.CENTER)
                //   .Add(new Paragraph("Concepto"));
                //Cell cell12 = new Cell(1, 1)
                //   .SetBackgroundColor(ColorConstants.GRAY)
                //   .SetTextAlignment(TextAlignment.CENTER)
                //   .Add(new Paragraph("Costo"));

                //Cell cell21 = new Cell(1, 1)
                //   .SetTextAlignment(TextAlignment.CENTER)
                //   .Add(new Paragraph("Concepto 1"));
                //Cell cell22 = new Cell(1, 1)
                //   .SetTextAlignment(TextAlignment.CENTER)
                //   .Add(new Paragraph("10"));

                //Cell cell31 = new Cell(1, 1)
                //   .SetTextAlignment(TextAlignment.CENTER)
                //   .Add(new Paragraph("Concepto 2"));
                //Cell cell32 = new Cell(1, 1)
                //   .SetTextAlignment(TextAlignment.CENTER)
                //   .Add(new Paragraph("20"));

                //table.AddCell(cell11);
                //table.AddCell(cell12);
                //table.AddCell(cell21);
                //table.AddCell(cell22);
                //table.AddCell(cell31);
                //table.AddCell(cell32);

                //document.Add(table); 
                #endregion

                int i = 4;
                dataDocumento.contenido.ForEach(contenido =>
                {
                    Text text = new Text(contenido)
                    .SetFont(fuenteExterna);
                    Paragraph pContenido = new Paragraph(text)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFontSize(10);
                    cellContent = new Cell(i, 1);
                    cellContent.Add(pContenido);
                    cellContent.SetBorder(Border.NO_BORDER);
                    table.AddCell(cellContent);
                    i++;
                });

                Paragraph leyenda = new Paragraph(dataDocumento.pie)
                   .SetTextAlignment(TextAlignment.LEFT)
                   .SetFontSize(8);
                cellContent = new Cell(i, 1);
                cellContent.Add(leyenda);
                cellContent.SetBorder(Border.NO_BORDER);
                table.AddCell(cellContent);

                table.IsComplete();
                table.SetFixedLayout();
                string fileName = "c:\\documentosGMF\\" + Guid.NewGuid() + ".pdf";
                PdfWriter writer = new PdfWriter(fileName);
                PdfDocument pdf = new PdfDocument(writer);

                Document documentAux = new Document(pdf);

                float necessaryWidth = 300f;
                IRenderer tableRenderer = table.CreateRendererSubTree().SetParent(documentAux.GetRenderer());
                LayoutResult tableLayoutResult = tableRenderer.Layout(new LayoutContext(new LayoutArea(0, new Rectangle(necessaryWidth, 1000))));
                float tableHeightTotal = tableLayoutResult.GetOccupiedArea().GetBBox().GetHeight();


                PageSize pageSize = new PageSize(300f, tableHeightTotal);
                Document document = new Document(pdf, pageSize);
                document.SetMargins(0, 0, 0, 20);
                document.Add(table);
                document.Close();
                documentAux.Close();

                response.State = ResponseType.Success;
                response.Message = fileName;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public Response GenerarDocumentoTabla(DataDocumento dataDocumento)
        {
            Response response = new Response();
            try
            {
                Table table = new Table(1, false);
                table.SetWidth(520);
                table.SetBorder(Border.NO_BORDER);

                string pathLogo = string.IsNullOrEmpty(dataDocumento.pathLogo) ? @"c:\fonts\Logo.jpg" : dataDocumento.pathLogo;
                Image img = new Image(ImageDataFactory
               .Create(pathLogo))
               .SetHeight(60)
               .SetWidth(150)
               .SetTextAlignment(TextAlignment.CENTER);
                Cell cellContent = new Cell(1, 1);
                cellContent.Add(img);
                cellContent.SetBorder(Border.NO_BORDER);
                table.AddCell(cellContent);

                FontProgram fontProgram = FontProgramFactory.CreateFont(@"c:\fonts\arial.ttf");
                PdfFont fuenteExterna = PdfFontFactory.CreateFont(fontProgram, PdfEncodings.WINANSI);

                Text text = new Text(dataDocumento.titulo)
                    .SetFont(fuenteExterna)
                    .SetBold();

                Paragraph subheader = new Paragraph(text)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(14);
                cellContent = new Cell(2, 1);
                cellContent.Add(subheader);
                cellContent.SetBorder(Border.NO_BORDER);
                table.AddCell(cellContent);

                LineSeparator lineSeparator = new LineSeparator(new SolidLine());
                cellContent = new Cell(3, 1);
                cellContent.Add(lineSeparator);
                cellContent.SetBorder(Border.NO_BORDER);
                table.AddCell(cellContent);

                #region  para hacer tablas
                ///TODO titulos
                Table tableContent = new Table(dataDocumento.titulosTabla.Count, false);
                tableContent.SetWidth(520);
                tableContent.SetTextAlignment(TextAlignment.CENTER);
                dataDocumento.titulosTabla.ForEach(x =>
                {
                    Cell cellTitle = new Cell(1, 1)
                       .SetBackgroundColor(ColorConstants.GRAY)
                       .SetTextAlignment(TextAlignment.CENTER)
                       .Add(new Paragraph(x));
                    tableContent.AddCell(cellTitle);
                });
                ///TODO contenido
                dataDocumento.contenido.ForEach(x =>
                {
                    List<string> contenidoInner = x.Split('|').ToList();
                    contenidoInner.ForEach(yy =>
                    {
                        decimal resulParse = 0;

                        NumberStyles style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
                        CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                        TextAlignment textAlignment = decimal.TryParse(yy.Trim(), style, culture, out resulParse) == true ? TextAlignment.RIGHT : TextAlignment.CENTER;
                        yy = decimal.TryParse(yy.Trim(), style, culture, out resulParse) == true ? resulParse.ToString("C", culture).Replace("$", "") : yy;

                        Cell cellInnerContent = new Cell(1, 1)
                           .SetTextAlignment(textAlignment)
                           .Add(new Paragraph(yy));
                        tableContent.AddCell(cellInnerContent);
                    });

                });
                #endregion
                string pathRootDocument = "c:\\documentosGMF\\";
                if (!Directory.Exists(pathRootDocument))
                {
                    Directory.CreateDirectory(pathRootDocument);
                }

                string fileName = pathRootDocument + Guid.NewGuid() + ".pdf";
                PdfWriter writer = new PdfWriter(fileName);
                PdfDocument pdf = new PdfDocument(writer);

                Paragraph leyenda = new Paragraph(dataDocumento.pie)
                  .SetTextAlignment(TextAlignment.LEFT)
                  .SetFontSize(8);

                Document document = new Document(pdf, PageSize.LETTER);
                document.SetMargins(10, 10, 10, 10);
                document.Add(table);
                document.Add(tableContent);
                document.Add(leyenda);
                document.Close();

                response.State = ResponseType.Success;
                response.Message = fileName;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        /// <summary>Ancho imprimible del papel de 80mm (72mm) en puntos PDF.</summary>
        private const float ANCHO_TERMICO_PT = 204f;
        private const float MARGEN_TERMICO_PT = 4f;
        /// <summary>Alto maximo de pagina PDF; un reporte mas largo continua en otra pagina.</summary>
        private const float ALTO_MAXIMO_PT = 14400f;

        /// <summary>
        /// Genera una tabla para impresora termica de 80mm, en una sola pagina del alto del contenido.
        /// Convenciones de cada linea de <c>contenido</c> (celdas separadas por '|'):
        /// una linea sin '|' es un titulo de grupo que ocupa toda la fila, y una linea que empieza
        /// con '*' es un subtotal o total y va en negrita. Las celdas numericas se alinean a la derecha.
        /// </summary>
        public Response GenerarDocumentoTablaTermica(DataDocumento dataDocumento)
        {
            Response response = new Response();
            try
            {
                List<string> titulos = dataDocumento.titulosTabla ?? new List<string>();
                if (titulos.Count == 0)
                {
                    throw new ArgumentException("La tabla termica requiere al menos un titulo de columna.");
                }

                int columnas = titulos.Count;
                List<float> anchos = dataDocumento.anchosColumnas?.Count == columnas
                    ? dataDocumento.anchosColumnas
                    : Enumerable.Repeat(1f, columnas).ToList();

                List<string[]> filas = (dataDocumento.contenido ?? new List<string>())
                    .Select(x => (x ?? string.Empty).Split('|'))
                    .ToList();

                NumberStyles estiloNumero = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign;
                CultureInfo cultura = CultureInfo.CreateSpecificCulture("en-US");
                bool EsNumero(string valor) => decimal.TryParse(valor.Trim().TrimStart('*'), estiloNumero, cultura, out _);

                // Una columna es numerica si alguna de sus celdas lo es; asi la cabecera se alinea con sus datos.
                bool[] columnaNumerica = new bool[columnas];
                filas.Where(f => f.Length > 1).ToList().ForEach(f =>
                {
                    for (int c = 0; c < Math.Min(columnas, f.Length); c++)
                    {
                        columnaNumerica[c] = columnaNumerica[c] || (f[c].Trim().Length > 0 && EsNumero(f[c]));
                    }
                });

                FontProgram fontProgram = FontProgramFactory.CreateFont(@"c:\fonts\arial.ttf");
                PdfFont fuente = PdfFontFactory.CreateFont(fontProgram, PdfEncodings.WINANSI);
                Div contenedor = new Div().SetFont(fuente).SetFontSize(8);

                string pathLogo = string.IsNullOrEmpty(dataDocumento.pathLogo) ? @"c:\fonts\Logo.jpg" : dataDocumento.pathLogo;
                if (File.Exists(pathLogo))
                {
                    contenedor.Add(new Image(ImageDataFactory.Create(pathLogo))
                        .SetHeight(40)
                        .SetWidth(100)
                        .SetHorizontalAlignment(HorizontalAlignment.CENTER));
                }

                contenedor.Add(new Paragraph(dataDocumento.titulo ?? string.Empty)
                    .SetBold()
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(4));

                Table tabla = new Table(UnitValue.CreatePercentArray(anchos.ToArray()))
                    .SetWidth(UnitValue.CreatePercentValue(100))
                    .SetFixedLayout();

                Border lineaFina = new SolidBorder(ColorConstants.BLACK, 0.5f);
                for (int c = 0; c < columnas; c++)
                {
                    tabla.AddHeaderCell(new Cell()
                        .Add(new Paragraph(titulos[c]).SetBold())
                        .SetTextAlignment(columnaNumerica[c] ? TextAlignment.RIGHT : TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)
                        .SetBorderTop(lineaFina)
                        .SetBorderBottom(lineaFina)
                        .SetPadding(1));
                }

                foreach (string[] fila in filas)
                {
                    if (fila.Length == 1)
                    {
                        tabla.AddCell(new Cell(1, columnas)
                            .Add(new Paragraph(fila[0].Trim()).SetBold())
                            .SetBorder(Border.NO_BORDER)
                            .SetPaddingTop(4)
                            .SetPaddingBottom(1)
                            .SetPaddingLeft(1));
                        continue;
                    }

                    bool esTotal = fila[0].StartsWith("*");
                    for (int c = 0; c < columnas; c++)
                    {
                        string valor = c < fila.Length ? fila[c].Trim() : string.Empty;
                        if (c == 0 && esTotal)
                        {
                            valor = valor.TrimStart('*').Trim();
                        }

                        Paragraph parrafo = new Paragraph(valor);
                        if (esTotal)
                        {
                            parrafo.SetBold();
                        }

                        Cell celda = new Cell()
                            .Add(parrafo)
                            .SetTextAlignment(columnaNumerica[c] && valor.Length > 0 && EsNumero(valor) ? TextAlignment.RIGHT : TextAlignment.LEFT)
                            .SetBorder(Border.NO_BORDER)
                            .SetPadding(1);
                        if (esTotal)
                        {
                            celda.SetBorderTop(lineaFina);
                        }
                        tabla.AddCell(celda);
                    }
                }

                contenedor.Add(tabla);
                contenedor.Add(new Paragraph(dataDocumento.pie ?? string.Empty)
                    .SetFontSize(7)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginTop(6));

                string pathRootDocument = "c:\\documentosGMF\\";
                if (!Directory.Exists(pathRootDocument))
                {
                    Directory.CreateDirectory(pathRootDocument);
                }
                string fileName = pathRootDocument + Guid.NewGuid() + ".pdf";

                using (PdfDocument pdf = new PdfDocument(new PdfWriter(fileName)))
                {
                    // Se mide el contenido para que la pagina tenga el alto exacto del ticket.
                    float anchoUtil = ANCHO_TERMICO_PT - (2 * MARGEN_TERMICO_PT);
                    Document medicion = new Document(pdf);
                    IRenderer renderer = contenedor.CreateRendererSubTree().SetParent(medicion.GetRenderer());
                    LayoutResult resultado = renderer.Layout(new LayoutContext(new LayoutArea(1, new Rectangle(anchoUtil, ALTO_MAXIMO_PT))));
                    float alto = resultado.GetStatus() == LayoutResult.FULL
                        ? resultado.GetOccupiedArea().GetBBox().GetHeight() + (2 * MARGEN_TERMICO_PT)
                        : ALTO_MAXIMO_PT;

                    Document document = new Document(pdf, new PageSize(ANCHO_TERMICO_PT, Math.Min(alto, ALTO_MAXIMO_PT)));
                    document.SetMargins(MARGEN_TERMICO_PT, MARGEN_TERMICO_PT, MARGEN_TERMICO_PT, MARGEN_TERMICO_PT);
                    document.Add(contenedor);
                    document.Close();
                }

                response.State = ResponseType.Success;
                response.Message = fileName;
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

    }
}
