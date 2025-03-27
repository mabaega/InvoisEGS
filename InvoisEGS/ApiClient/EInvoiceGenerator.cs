using InvoisEGS.ApiClient.ApiHelpers;
using InvoisEGS.ApiClient.ApiModels;
using InvoisEGS.ApiClient.XModels;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Newtonsoft.Json;

namespace InvoisEGS.ApiClient
{
    public class EInvoiceGenerator
    {
        public string X509CertificateContent { get; set; }
        public string RsaPrivkeypem { get; set; }

        public EInvoiceGenerator(string x509CertificateContent, string rsaPrivkeypem)
        {
            try
            {
                X509CertificateContent = x509CertificateContent;
                RsaPrivkeypem = Encoding.UTF8.GetString(Convert.FromBase64String(rsaPrivkeypem));
            }catch (Exception ex)
            {
                //throw;
            }
            
        }

        public SubmitDocumentRequest GenerateEInvoiceXML(MyInvoice invObject, bool signDocument = false)
        {
            if (invObject == null)
                throw new ArgumentNullException(nameof(invObject));
            try
            {
                string codeNumber = invObject.ID[0].Value;
 
                // Convert invoice object to XML and canonicalize
                string cleanInvoice = XmlUtility.SerializeToXml(invObject);
                string canonicalXml = XmlUtility.Canonicalize(cleanInvoice);

                // Calculate document hash
                byte[] documentBytes = Encoding.UTF8.GetBytes(canonicalXml);
                using var sha256 = System.Security.Cryptography.SHA256.Create();
                byte[] documentHash = sha256.ComputeHash(documentBytes);
                string documentDigestBase64 = Convert.ToBase64String(documentHash);

                string finalData = canonicalXml;
                string recHash = XmlJsonUtilities.ComputeHexHash(documentBytes);

                if (signDocument && !string.IsNullOrEmpty(X509CertificateContent) && !string.IsNullOrEmpty(RsaPrivkeypem))
                {
                    // Process certificate first
                    byte[] certificateBytes = Convert.FromBase64String(X509CertificateContent);
                    using X509Certificate2 cert = new(certificateBytes);

                    // Get certificate info
                    string certDigest = XmlJsonUtilities.ComputeHash(cert.RawData);
                    string issuerName = XmlJsonUtilities.SecurityElement.Escape(cert.IssuerName.Name);
                    string certSerialNumber = XmlJsonUtilities.GetCertificateSerialNumberAsDecimal(cert);
                    string rawData = Convert.ToBase64String(cert.RawData);
                    string utcTimestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

                    // Create and process signed properties
                    string signedPropsXml = XmlUtility.GetSignedPropertiesHash(utcTimestamp, certDigest, issuerName, certSerialNumber);
                    string canonicalSignedProps = XmlUtility.Canonicalize(signedPropsXml);
                    byte[] signedPropsBytes = Encoding.UTF8.GetBytes(canonicalSignedProps);
                    string signedPropsDigest = Convert.ToBase64String(sha256.ComputeHash(signedPropsBytes));

                    // Sign the document hash (using hash bytes directly)
                    string signatureBase64 = XmlJsonUtilities.SignHash(documentHash, RsaPrivkeypem);

                    // Load and process UBL Extensions template
                    string signatureTemplate = XmlJsonUtilities.LoadEmbededResources("InvoisEGS.ApiClient.Resources.XmlUBLExtensions.txt");
                    string ublTemplate = signatureTemplate
                        .Replace("${DOCUMENT_DIGEST}", documentDigestBase64)
                        .Replace("${SIGNED_PROPS_DIGEST}", signedPropsDigest)
                        .Replace("${SIGNATURE_VALUE}", signatureBase64)
                        .Replace("${CERTIFICATE}", rawData)
                        .Replace("${TIMESTAMP}", utcTimestamp)
                        .Replace("${CERT_DIGEST}", certDigest)
                        .Replace("${ISSUER_NAME}", XmlJsonUtilities.SecurityElement.Escape(issuerName))
                        .Replace("${SERIAL_NUMBER}", certSerialNumber);

                    // Insert first signature (UBL Extensions)
                    string[] lines = cleanInvoice.Split('\n');
                    cleanInvoice = lines[0] + "\n" + ublTemplate + "\n" + string.Join("\n", lines.Skip(1));

                    // Load and insert second signature template
                    string signatureTemplate2 = XmlJsonUtilities.LoadEmbededResources("InvoisEGS.ApiClient.Resources.XmlSignature.txt");
                    cleanInvoice = cleanInvoice.Replace("  <cac:AccountingSupplierParty>", signatureTemplate2);

                    // Calculate final hash after both signatures
                    finalData = XmlUtility.Canonicalize(cleanInvoice);
                    recHash = XmlJsonUtilities.ComputeHexHash(Encoding.UTF8.GetBytes(finalData));
                }

                byte[] bytes = Encoding.UTF8.GetBytes(finalData);
                string base64Invoice = Convert.ToBase64String(bytes);
                // Create API request
                return new SubmitDocumentRequest
                {
                    Documents = new OneDocument[]
                    {
                        new()
                        {
                            Format = "XML",
                            DocumentHash = recHash,
                            CodeNumber = codeNumber,
                            Document = base64Invoice
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public SubmitDocumentRequest GenerateEInvoiceJSON(MyInvoice invoiceObject, bool signDocument = false)
        {
            if (invoiceObject == null)
                throw new ArgumentNullException(nameof(invoiceObject));

            try
            {
                string codeNumber = invoiceObject.ID[0].Value;
                string finalData;
                string recHash;

                // Convert invoice object to JSON and minify
                NamespaceRoot rootObject = new NamespaceRoot();
                rootObject.MyInvoice.Add(invoiceObject);
                string invoiceJson = JsonConvert.SerializeObject(rootObject);
                invoiceJson = JsonUtility.RemoveNullElements(invoiceJson);
                string minifiedJson = JsonUtility.MinifyJson(invoiceJson);

                if (signDocument && !string.IsNullOrEmpty(X509CertificateContent) && !string.IsNullOrEmpty(RsaPrivkeypem))
                {
                    // 1. Calculate document hash (seperti di PowerShell)
                    using var sha256 = System.Security.Cryptography.SHA256.Create();
                    byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(minifiedJson));
                    string documentDigestBase64 = Convert.ToBase64String(hash);

                    // 2. Load certificate
                    byte[] certificateBytes = Convert.FromBase64String(X509CertificateContent);
                    using X509Certificate2 cert = new(certificateBytes);

                    // 3. Calculate certificate digest
                    string certDigest = XmlJsonUtilities.ComputeHash(cert.RawData);
                    string issuerName = XmlJsonUtilities.SecurityElement.Escape(cert.IssuerName.Name); // Only escape issuerName
                    string certSerialNumber = XmlJsonUtilities.GetCertificateSerialNumberAsDecimal(cert);
                    string subjectName = cert.SubjectName.Name; // No escape for subjectName
                    string rawData = Convert.ToBase64String(cert.RawData);
                    string utcTimestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

                    // 4. Create and hash SignedProperties
                    string signedPropsJson = JsonUtility.GenerateSignedPropertiesJson(utcTimestamp, certDigest, issuerName, certSerialNumber);
                    string minifiedSignedProps = JsonUtility.MinifyJson(signedPropsJson);
                    byte[] signedPropsHashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(minifiedSignedProps));
                    string signedPropsDigest = Convert.ToBase64String(signedPropsHashBytes);

                    // 5. Sign the document hash (menggunakan hash langsung, bukan Base64-nya)
                    string signatureBase64 = XmlJsonUtilities.SignHash(hash, RsaPrivkeypem);

                    // 6. Combine final JSON
                    string signString = JsonUtility.GenerateSignString(
                        utcTimestamp, certDigest, issuerName, certSerialNumber, 
                        rawData, subjectName, signatureBase64, signedPropsDigest, documentDigestBase64);

                    string modifiedContent = minifiedJson.Substring(0, minifiedJson.Length - 3);
                    string combinedData = modifiedContent + "," + signString + "}]}";
                    finalData = JsonUtility.MinifyJson(combinedData);

                    // 7. Calculate final hash for API request
                    recHash = XmlJsonUtilities.ComputeHexHash(Encoding.UTF8.GetBytes(finalData));
                }
                else
                {
                    finalData = minifiedJson;
                    recHash = XmlJsonUtilities.ComputeHexHash(Encoding.UTF8.GetBytes(finalData));
                }

                // Create API request
                return new SubmitDocumentRequest
                {
                    Documents = new OneDocument[]
                    {
                        new()
                        {
                            Format = "JSON",
                            DocumentHash = recHash,
                            CodeNumber = codeNumber,
                            Document = Convert.ToBase64String(Encoding.UTF8.GetBytes(finalData))
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                throw; //new EInvoiceGenerationException("Error generating invoice API request", ex);
            }
        }

    }
}