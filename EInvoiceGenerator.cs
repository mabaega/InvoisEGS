using InvoisEGS.ApiClient.Helpers;
using InvoisEGS.ApiClient.Models;
using InvoisEGS.ApiClient.Models.EInvoice;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace InvoisEGS.ApiClient
{
    public class EInvoiceGenerator
    {
        public string X509CertificateContent { get; set; }
        public string EcSecp256k1Privkeypem { get; set; }

        public EInvoiceGenerator(string x509CertificateContent, string ecSecp256k1Privkeypem)
        {
            X509CertificateContent = x509CertificateContent;
            EcSecp256k1Privkeypem = Encoding.UTF8.GetString(Convert.FromBase64String(ecSecp256k1Privkeypem));
        }

        public SubmitDocumentRequest GenerateEInvoice(Invoice invObject, bool signDocument = false)
        {
            string codeNumber = invObject.ID.Text;

            string cleanInvoice = XmlUtility.GetCleanInvoiceXML(invObject);

            string invoiceHash = Helpers.Utilities.GetBase64InvoiceHash(cleanInvoice);

            if (signDocument && !string.IsNullOrEmpty(X509CertificateContent) && !string.IsNullOrEmpty(EcSecp256k1Privkeypem))
            {
                string signatureValue = Helpers.Utilities.GetDigitalSignature(invoiceHash, EcSecp256k1Privkeypem);
                byte[] certificateBytes = Convert.FromBase64String(X509CertificateContent);
                X509Certificate2 parsedCertificate = new(certificateBytes);

                string publicKeyHashing = Convert.ToBase64String(Encoding.UTF8.GetBytes(Helpers.Utilities.HashSha256AsString(X509CertificateContent)));
                string signatureTimestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
                string issuerName = parsedCertificate.Issuer;
                string serialNumber = InvoisEGS.ApiClient.Helpers.Utilities.GetSerialNumberForCertificateObject(parsedCertificate);
                string signedPropertiesHash = Helpers.Utilities.GetSignedPropertiesHash(signatureTimestamp, publicKeyHashing, issuerName, serialNumber);

                UBLExtension ublExtension = new(invoiceHash,
                    signedPropertiesHash,
                    signatureValue,
                    X509CertificateContent,
                    signatureTimestamp,
                    publicKeyHashing,
                    issuerName,
                    serialNumber);

                string[] lines = cleanInvoice.Split('\n');
                cleanInvoice = lines[0] + "\n" + ublExtension.ToString() + "\n" + string.Join("\n", lines.Skip(1));

                string signatreTemplate = InvoisEGS.ApiClient.Helpers.Utilities.LoadEmbededResources("InvoisEGS.ApiClient.Resources.SignatureTemplate.txt");
                cleanInvoice = cleanInvoice.Replace("  <cac:AccountingSupplierParty>", signatreTemplate);
            }

            byte[] bytes = Encoding.UTF8.GetBytes(cleanInvoice);
            string base64SignedInvoice = Convert.ToBase64String(bytes);

            SubmitDocumentRequest submitDocumentRequest = new()
            {
                Documents = new OneDocument[]
                {
                    new() {
                        Format = "XML",
                        DocumentHash = invoiceHash,
                        CodeNumber = codeNumber,
                        Document = base64SignedInvoice
                    }
                }
            };

            return submitDocumentRequest;
        }
    }
}