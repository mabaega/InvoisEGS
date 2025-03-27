using MyInvois.ApiClient.Helpers;
using MyInvois.ApiClient.Models;
using MyInvois.ApiClient.Models.EInvoice;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MyInvois.ApiClient
{
    public class EInvoiceGenerator
    {
        public string X509CertificateContent { get; set; }
        public string EcSecp256k1Privkeypem { get; set; }

        public EInvoiceGenerator(string x509CertificateContent, string ecSecp256k1Privkeypem)
        {
            X509CertificateContent = x509CertificateContent;
            EcSecp256k1Privkeypem = ecSecp256k1Privkeypem;
        }

        public SubmitDocumentRequest GenerateSignedInvoice(Invoice invObject)
        {
            string codeNumber = invObject.ID.Text;

            // Step 3: Canonicalize the document and generate the document hash (digest)
            // 3.1 Prepare document canonical version, to be used for signing
            string cleanInvoice = XmlUtility.CleanUpInvoiceXML(invObject);
            cleanInvoice = cleanInvoice.Replace("\r\n", "\n");
            cleanInvoice = cleanInvoice.Replace("\n", "\r\n");

            // 3.2 Hash the canonicalized document invoice body using SHA-256
            // Use a HEX-to-Base64 encoder to encode the hashed value and convert it to base 64.
            string invoiceHash = Utilities.GetBase64InvoiceHash(cleanInvoice);

            // Step 4: Sign the document digest
            // 4.1 Sign the generated invoice hash with RSA-SHA256 using the signing certificate private key
            // 4.2 The output will be set as the value of the property Sig
            // 4.3 Encode the hashed property tag using Base64 Encoder
            string signatureValue = Utilities.GetDigitalSignature(invoiceHash, EcSecp256k1Privkeypem);

            // Step 5: Generate the certificate hash
            byte[] certificateBytes = Convert.FromBase64String(X509CertificateContent);
            X509Certificate2 parsedCertificate = new(certificateBytes);

            // 5.1 Hash the signing certificate using SHA-256
            // 5.2 Encode the certificate hash using Base64 encoding
            string publicKeyHashing = Convert.ToBase64String(Encoding.UTF8.GetBytes(Utilities.HashSha256AsString(X509CertificateContent)));

            // Step 6: Populate the signed properties section
            // DigestValue > Encoded certificate hashed property reference CertDigest
            // SigningTime > Sign timestamp as current datetime in UTC
            string signatureTimestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
            // X509SupplierName > Signing certificate Supplier name
            string SupplierName = parsedCertificate.Issuer.ToString();
            // X509SerialNumber > Signing certificate serial number
            string serialNumber = Utilities.GetSerialNumberForCertificateObject(parsedCertificate);
            // Now the signing properties section should be ready to be hashed.

            //Step 7: Generate Signed Properties Hash
            string signedPropertiesHash = Utilities.GetSignedPropertiesHash(signatureTimestamp, publicKeyHashing, SupplierName, serialNumber);

            // Step 8: Populate the information in the document to create the signed document
            UBLExtension ublExtension = new(invoiceHash,
                signedPropertiesHash,
                signatureValue,
                X509CertificateContent,
                signatureTimestamp,
                publicKeyHashing,
                SupplierName,
                serialNumber);


            int profileIDIndex = cleanInvoice.IndexOf("<cbc:ProfileID>");
            cleanInvoice = cleanInvoice.Insert(profileIDIndex - 6, ublExtension.ToString());

            int accountingSupplierPartyIndex = cleanInvoice.IndexOf("<cac:AccountingSupplierParty>");

            //CleanInvoice = CleanInvoice.Insert(AccountingSupplierPartyIndex - 6, stringXMLQrCode);

            cleanInvoice = cleanInvoice.Insert(accountingSupplierPartyIndex - 6, @"Resources\SignatureTemplate.txt");

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