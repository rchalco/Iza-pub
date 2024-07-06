using PlumbingProps.CrossUtil;
using PlumbingProps.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Invoice.Core.Base;
using PlumbingProps.Config;

namespace Invoice.Core.Artefacts.Invoice
{
    public class Signer : BaseManager
    {
        private static string TMP_DIRECTORY = ConfigManager.GetConfiguration().GetSection("Directories:InvoiceSignFolderTemporal").Value!;

        public ResponseObject<string> FirmarDocumentoXML(string xmlContent, string fileRSAKey, string fileX509)
        {
            ResponseObject<string> responseObject = new ResponseObject<string>
            {
                State = ResponseType.Success,
                Message = "Documento firmado correctamente",
                Data = "",
            };
            try
            {
                if (string.IsNullOrEmpty(fileRSAKey) || string.IsNullOrEmpty(fileX509))
                {
                    throw new ArgumentException("Los parametros fileRSAKey y fileX509 no debe ser vacios o nulos ");
                }
                string rsaPEM = File.ReadAllText(fileRSAKey);
                string x059PEM = File.ReadAllText(fileX509);
                var rsa = RSA.Create();
                rsa.ImportFromPem(rsaPEM.ToCharArray());
                var x509Certificate = X509Certificate2.CreateFromPem(x059PEM);
                if (!Directory.Exists(TMP_DIRECTORY))
                {
                    Directory.CreateDirectory(TMP_DIRECTORY);
                }
                string nameFile = Path.Combine(TMP_DIRECTORY, CustomGuid.GetGuid() + ".xml");
                SignXmlFile(xmlContent, nameFile, rsa, x509Certificate);
                responseObject.Data = File.ReadAllText(nameFile);
            }
            catch (Exception ex)
            {
                ProcessError(ex, responseObject);
            }
            return responseObject;
        }

        private void SignXmlFile(string content, string SignedFileName, RSA Key, X509Certificate x509Certificate)
        {
            // Create a new XML document.
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            // Load the passed XML file using its name.
            doc.LoadXml(content);

            // Create a SignedXml object.
            SignedXml signedXml = new SignedXml(doc);

            // Add the key to the SignedXml document. 
            signedXml.SigningKey = Key;

            // Create a reference to be signed.
            Reference reference = new Reference();
            reference.Uri = "";

            // Add an enveloped transformation to the reference.
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            // Add the reference to the SignedXml object.
            signedXml.AddReference(reference);

            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(x509Certificate));
            signedXml.KeyInfo = keyInfo;

            // Compute the signature.
            signedXml.ComputeSignature();

            // Add prefix "ds:" to signature
            XmlElement signature = signedXml.GetXml();
            SetPrefix("ds", signature);

            // Load modified signature back
            signedXml.LoadXml(signature);

            // this is workaround for overcoming a bug in the library
            signedXml.SignedInfo.References.Clear();

            // Recompute the signature
            signedXml.ComputeSignature();
            string recomputedSignature = Convert.ToBase64String(signedXml.SignatureValue);

            // Replace value of the signature with recomputed one
            ReplaceSignature(signature, recomputedSignature);

            //// Append the element to the XML document.
            doc.DocumentElement.AppendChild(doc.ImportNode(signature, true));

            if (doc.FirstChild is XmlDeclaration)
            {
                doc.RemoveChild(doc.FirstChild);
            }

            // Save the signed XML document to a file specified
            // using the passed string.
            XmlTextWriter xmltw = new XmlTextWriter(SignedFileName, new UTF8Encoding(false));
            doc.WriteTo(xmltw);
            xmltw.Close();
        }

        private static void SetPrefix(string prefix, XmlNode node)
        {
            node.Prefix = prefix;
            foreach (XmlNode n in node.ChildNodes)
            {
                SetPrefix(prefix, n);
            }
        }

        private static void ReplaceSignature(XmlElement signature, string newValue)
        {
            if (signature == null) throw new ArgumentNullException(nameof(signature));
            if (signature.OwnerDocument == null) throw new ArgumentException("No owner document", nameof(signature));

            XmlNamespaceManager nsm = new XmlNamespaceManager(signature.OwnerDocument.NameTable);
            nsm.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);

            XmlNode signatureValue = signature.SelectSingleNode("ds:SignatureValue", nsm);
            if (signatureValue == null)
                throw new Exception("Signature does not contain 'ds:SignatureValue'");

            signatureValue.InnerXml = newValue;
        }
    }
}
