using MyInvois.ApiClient.Helpers;
using MyInvois.ApiClient.Models.EInvoice;

namespace MyInvois
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Set console encoding to UTF8
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Ensure UTF-8 encoding is used for all text operations
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // Path to the sample XML file
            string filePath = @"C:\Users\Incredible One\source\repos\MyInvoisEGS\MyInvoice\Documents\1.0-Invoice-Sample.xml";

            try
            {
                // Load the invoice from XML file
                Invoice invoice = XmlUtility.LoadFromFile(filePath);

                if (invoice == null)
                {
                    Console.WriteLine("Failed to load invoice: The deserialized object is null.");
                    return;
                }

                Console.WriteLine("Invoice successfully loaded from XML file.");

                // Serialize the invoice back to XML string
                string xmlString = XmlUtility.SerializeInvoice(invoice);
                Console.WriteLine("\nSerialized XML:");
                Console.WriteLine(xmlString);

                // Deserialize the XML string back to object
                Invoice deserializedInvoice = XmlUtility.DeserializeInvoice(xmlString);

                if (deserializedInvoice == null)
                {
                    Console.WriteLine("Failed to deserialize invoice: The deserialized object is null.");
                    return;
                }

                Console.WriteLine("\nInvoice successfully deserialized from XML string.");

                // Save the invoice to a new file
                string newFilePath = @"Documents\1.0-Invoice-Sample-Output.xml";
                XmlUtility.SaveToFile(deserializedInvoice, newFilePath);
                Console.WriteLine($"\nInvoice saved to new file: {newFilePath}");


                // CertificateHandler.SaveTestCertificate(
                //     tin: "C20830570210",
                //     brn: "202005123456",
                //     companyName: "Test Company Sdn Bhd",
                //     outputPath: @"Documents\test_certificate.pfx",
                //     password: "password"
                // );

                // Load and verify the certificate
                System.Security.Cryptography.X509Certificates.X509Certificate2 cert = CertificateHandler.LoadCertificate(@"Documents\test_certificate.pfx", "password");
                Console.WriteLine($"Subject: {cert.Subject}");
                Console.WriteLine($"Valid from: {cert.NotBefore} to {cert.NotAfter}");

            }
            catch (InvalidOperationException ex) when (ex.InnerException != null)
            {
                Console.WriteLine($"XML Serialization Error: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }
    }
}
