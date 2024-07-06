using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using PlumbingProps.CrossUtil;
using QRCoder;
using System.Drawing;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using TarEntry = ICSharpCode.SharpZipLib.Tar.TarEntry;

namespace Invoice.Core.Artefacts.Invoice
{
    public class FacturaHelper
    {
        public static string pFormatoDate = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff";
        public void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    //msi.CopyTo(gs);
                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        public string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }

        }

        public string sha256_hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public string ConvertArryaBytesToSha256(byte[] value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {

                byte[] result = hash.ComputeHash(value);

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public string calculaDigitoMod11ER(string cadena, int numDig, int limMult, bool x10)

        {

            int mult, suma, i, n, dig;

            if (!x10) numDig = 1;

            for (n = 1; n <= numDig; n++)
            {

                suma = 0;

                mult = 2;

                for (i = cadena.Length - 1; i >= 0; i--)
                {

                    suma += mult * int.Parse(cadena.Substring(i, 1)); ///verificar

                    if (++mult > limMult) mult = 2;

                }

                if (x10)
                {

                    dig = suma * 10 % 11 % 10;

                }

                else
                {

                    dig = suma % 11;

                }

                if (dig == 10)
                {

                    cadena += "1";

                }

                if (dig == 11)
                {

                    cadena += "0";

                }

                if (dig < 10)
                {

                    cadena += Convert.ToString(dig); //VErificar

                }

            }

            return cadena.Substring(cadena.Length - numDig, 1);

        }

        public string calculaDigitoMod11GOF(string cadena, int numDig, int limMult, bool x10)
        {
            int mult, suma, i, n, dig;
            if (!x10) numDig = 1;

            for (n = 1; n <= numDig; n++)
            {
                suma = 0;
                mult = 2;
                for (i = cadena.Length - 1; i >= 0; i--)
                {
                    suma += mult * int.Parse(cadena.Substring(i, 1));

                    if (++mult > limMult) mult = 2;
                }

                if (x10)
                {
                    dig = suma * 10 % 11 % 10;
                }
                else
                {
                    dig = suma % 11;
                }

                if (dig == 10)
                {
                    cadena += "1";
                }

                if (dig == 11)
                {
                    cadena += "0";
                }

                if (dig < 10)
                {
                    cadena += dig.ToString();
                }
            }

            return cadena.Substring(cadena.Length - numDig, numDig);

        }

        public static string ConvertirBase16(string CodigoPrevio)
        {
            var bigNumber = System.Numerics.BigInteger.Parse(CodigoPrevio);
            string vR = bigNumber.ToString("X");
            return vR;
        }

        public void CrearArchivoXML(string pXML, string path)
        {
            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(pXML);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }

        public byte[] ConvertirArchivoAByte(string path)
        {
            return File.ReadAllBytes(path);
        }

        /// <summary>
        /// crea archivos empaquetaos tar.gz
        /// </summary>
        /// <param name="tgzFilename"> ubicacion y nombre de archivo que se creara Ejemplo: C://FacturaSINTar//Paquete.tar.gz      </param>
        /// <param name="sourceDirectory"> ubicacion dela carpeta a comprimir  Ejemplo : C://FaturasSIN//Paquete1</param>
        public void CrearTarGZ(string tgzFilename, string sourceDirectory)
        {
            Stream outStream = File.Create(tgzFilename);
            Stream gzoStream = new GZipOutputStream(outStream);
            TarArchive tarArchive = TarArchive.CreateOutputTarArchive(gzoStream);

            tarArchive.RootPath = sourceDirectory.Replace('\\', '/');
            if (tarArchive.RootPath.EndsWith("/"))
                tarArchive.RootPath = tarArchive.RootPath.Remove(tarArchive.RootPath.Length - 1);

            AddDirectoryFilesToTar(tarArchive, sourceDirectory, true, true);
            tarArchive.Close();

        }

        private void AddDirectoryFilesToTar(TarArchive tarArchive, string sourceDirectory, bool recurse, bool isRoot)
        {

            TarEntry tarEntry;

            if (!isRoot)
            {
                tarEntry = TarEntry.CreateEntryFromFile(sourceDirectory);
                tarArchive.WriteEntry(tarEntry, false);
            }

            string[] filenames = Directory.GetFiles(sourceDirectory);
            foreach (string filename in filenames)
            {
                tarEntry = TarEntry.CreateEntryFromFile(filename);
                Console.WriteLine(tarEntry.Name);
                tarArchive.WriteEntry(tarEntry, true);
            }

            if (recurse)
            {
                string[] directories = Directory.GetDirectories(sourceDirectory);
                foreach (string directory in directories)
                    AddDirectoryFilesToTar(tarArchive, directory, recurse, false);
            }
        }

        /// <summary>
        /// crea carpetas y archivos 
        /// </summary>
        /// <param name="FolderNameRoot">carpeta raiz</param>
        /// <param name="FolderName">carpeta dentro de la carpeta raiz</param>
        /// <param name="FileName">nombre de archivo a crear</param>
        /// <param name="Body">cuerpo del archivo (interior de l archivo)</param>
        /// <returns></returns>
        public string CreaCarpertaArchivoXML(string FolderNameRoot, string FolderName, string FileName, string Body)
        {
            string vResultPath = "";

            try
            {

                DateTime today = DateTime.Today;
                //creamos los directorios para guardar los resultados
                if (!Directory.Exists(@"C:\" + FolderName))
                    Directory.CreateDirectory(@"C:\" + FolderName);
                string vDir = @"C:\" + FolderNameRoot + @"\" + FolderName;// + @"\" + today.ToString("yyyyMMdd");
                if (!Directory.Exists(vDir))
                    Directory.CreateDirectory(vDir);
                if (FolderName != "")
                {
                    File.AppendAllText(@"C:\" + FolderNameRoot + @"\" + FolderName + @"\" + FileName + ".xml", Body.ToString());
                    vResultPath = @"C:\" + FolderNameRoot + @"\" + FolderName + @"\";
                }
                else
                {
                    File.AppendAllText(@"C:\" + FolderNameRoot + @"\" + @"\" + FileName + ".xml", Body.ToString());
                    vResultPath = @"C:\" + FolderNameRoot + @"\" + @"\";
                }

                vResultPath = vResultPath.Replace(@"\", @"\\");

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return vResultPath;
        }

        public string GenerarCUF(CUFDHeaderRequest ObjParam)
        {
            string vTimestamp = ObjParam.FechaEmisionFactura.ToString(pFormatoDate);

            string vCadena = FacturaHelper.ObtieneNumCerosIzq(ObjParam.NIT, 13) +
                                  FacturaHelper.ObtieneNumCerosIzq(ObjParam.FechaHora, 17) +
                                  FacturaHelper.ObtieneNumCerosIzq(ObjParam.Sucursal, 4) +
                                  FacturaHelper.ObtieneNumCerosIzq(ObjParam.Modalidad, 1) +
                                  FacturaHelper.ObtieneNumCerosIzq(ObjParam.TipoEmision, 1) +
                                  FacturaHelper.ObtieneNumCerosIzq(ObjParam.TipoFactura, 1) +
                                  FacturaHelper.ObtieneNumCerosIzq(ObjParam.TipoDocumentoSector, 2) +
                                  FacturaHelper.ObtieneNumCerosIzq(ObjParam.NumeroFactura, 10) +
                                  FacturaHelper.ObtieneNumCerosIzq(ObjParam.PuntoVenta, 4);
            string vDigito2 = calculaDigitoMod11(vCadena, 1, 9, false);
            int CodigoAutoverificador = Convert.ToInt16(vDigito2);


            ///SE obtiene codigo previo 
            string CodigoPrevio = FacturaHelper.ObtieneNumCerosIzq(ObjParam.NIT, 13) +
                                  FacturaHelper.ObtieneNumCerosIzq(ObjParam.FechaHora, 17) +
                                  FacturaHelper.ObtieneNumCerosIzq(ObjParam.Sucursal, 4) +
                                  FacturaHelper.ObtieneNumCerosIzq(ObjParam.Modalidad, 1) +
                                  FacturaHelper.ObtieneNumCerosIzq(ObjParam.TipoEmision, 1) +
                                  FacturaHelper.ObtieneNumCerosIzq(ObjParam.TipoFactura, 1) +
                                  FacturaHelper.ObtieneNumCerosIzq(ObjParam.TipoDocumentoSector, 2) +
                                  FacturaHelper.ObtieneNumCerosIzq(ObjParam.NumeroFactura, 10) +
                                  FacturaHelper.ObtieneNumCerosIzq(ObjParam.PuntoVenta, 4) +
                                  ObtieneNumCerosIzq(CodigoAutoverificador, 1);

            string vR = ConvertirBase16(CodigoPrevio);
            vR = vR + ObjParam.CUFDCodigoControl;
            return vR;
        }

        private static string calculaDigitoMod11(string cadena, int numDig, int limMult, bool x10)
        {
            int mult, suma, i, n, dig;
            if (!x10) numDig = 1;

            for (n = 1; n <= numDig; n++)
            {
                suma = 0;
                mult = 2;
                for (i = cadena.Length - 1; i >= 0; i--)
                {
                    suma += mult * int.Parse(cadena.Substring(i, 1));

                    if (++mult > limMult) mult = 2;
                }

                if (x10)
                {
                    dig = suma * 10 % 11 % 10;
                }
                else
                {
                    dig = suma % 11;
                }

                if (dig == 10)
                {
                    cadena += "1";
                }

                if (dig == 11)
                {
                    cadena += "0";
                }

                if (dig < 10)
                {
                    cadena += dig.ToString();
                }
            }

            return cadena.Substring(cadena.Length - numDig, numDig);

        }



        public static string ObtieneNumCerosIzq(long Numero, int TamCampo)
        {
            string vTamañoCampo = "D" + TamCampo.ToString();
            return Numero.ToString(vTamañoCampo);
        }

        public string enletras(string num)
        {
            string res, dec = "";
            Int64 entero;
            int decimales;
            double nro;

            try

            {
                nro = Convert.ToDouble(num);
            }
            catch
            {
                return "";
            }

            entero = Convert.ToInt64(Math.Truncate(nro));
            decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
            //if (decimales > 0)
            //{
            //    dec = " CON " + decimales.ToString() + "/100";
            //}
            if (decimales == 0)
                dec = "  00/100";
            else
                dec = " " + decimales.ToString() + "/100";

            res = toText(Convert.ToDouble(entero)) + dec + " Bolivianos";
            return res;
        }

        private string toText(double value)
        {
            string Num2Text = "";
            value = Math.Truncate(value);
            if (value == 0) Num2Text = "CERO";
            else if (value == 1) Num2Text = "UNO";
            else if (value == 2) Num2Text = "DOS";
            else if (value == 3) Num2Text = "TRES";
            else if (value == 4) Num2Text = "CUATRO";
            else if (value == 5) Num2Text = "CINCO";
            else if (value == 6) Num2Text = "SEIS";
            else if (value == 7) Num2Text = "SIETE";
            else if (value == 8) Num2Text = "OCHO";
            else if (value == 9) Num2Text = "NUEVE";
            else if (value == 10) Num2Text = "DIEZ";
            else if (value == 11) Num2Text = "ONCE";
            else if (value == 12) Num2Text = "DOCE";
            else if (value == 13) Num2Text = "TRECE";
            else if (value == 14) Num2Text = "CATORCE";
            else if (value == 15) Num2Text = "QUINCE";
            else if (value < 20) Num2Text = "DIECI" + toText(value - 10);
            else if (value == 20) Num2Text = "VEINTE";
            else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);
            else if (value == 30) Num2Text = "TREINTA";
            else if (value == 40) Num2Text = "CUARENTA";
            else if (value == 50) Num2Text = "CINCUENTA";
            else if (value == 60) Num2Text = "SESENTA";
            else if (value == 70) Num2Text = "SETENTA";
            else if (value == 80) Num2Text = "OCHENTA";
            else if (value == 90) Num2Text = "NOVENTA";
            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);
            else if (value == 100) Num2Text = "CIEN";
            else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) Num2Text = "QUINIENTOS";
            else if (value == 700) Num2Text = "SETECIENTOS";
            else if (value == 900) Num2Text = "NOVECIENTOS";
            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);
            else if (value < 1000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
            }

            else if (value == 1000000) Num2Text = "UN MILLON";
            else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);
            else if (value < 1000000000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
            }

            else if (value == 1000000000000) Num2Text = "UN BILLON";
            else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else
            {
                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
            return Num2Text;

        }

        public string GenerarQR(string data)
        {
            string resul = "";
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(20);
            string pathIMG = CustomGuid.GetGuid() + ".png";

            using (var ms = new MemoryStream(qrCodeAsPngByteArr))
            {
                var qrCodeImage = new Bitmap(ms);
                qrCodeImage.Save(pathIMG);
                var bytesImg = File.ReadAllBytes(pathIMG);
                resul = Convert.ToBase64String(bytesImg);
                File.Delete(pathIMG);
            }
            return resul;
        }
    }
}
