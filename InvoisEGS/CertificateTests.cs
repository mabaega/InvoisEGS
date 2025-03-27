using System;
using System.Security.Cryptography.X509Certificates;
using InvoisEGS.ApiClient.Helpers;

public static class CertificateTests
{
    public static void RunCertificateTests()
    {
        string pfxPath = @"C:\Users\Incredible One\source\repos\InvoisEGS\InvoisEGS\test_certificate.pfx";
        string password = "password";

        try
        {
            // Load certificate
            using var certificate = CertificateHandler.LoadCertificate(pfxPath, password);
            
            // Verify basic properties
            Console.WriteLine($"Certificate Subject: {certificate.Subject}");
            Console.WriteLine($"Has Private Key: {certificate.HasPrivateKey}");
            Console.WriteLine($"Valid From: {certificate.NotBefore}");
            Console.WriteLine($"Valid To: {certificate.NotAfter}");
            
            // Check if certificate is valid for signing
            bool hasNonRepudiation = certificate.Extensions
                .OfType<X509KeyUsageExtension>()
                .Any(x => x.KeyUsages.HasFlag(X509KeyUsageFlags.NonRepudiation));

            Console.WriteLine($"Can be used for signing (NonRepudiation): {hasNonRepudiation}");



            // Extract and verify certificate contents
            var (certContent, privateKeyContent) = CertificateHandler.GetCertificateContents(pfxPath, password);
            Console.WriteLine("\nCertificate content available: " + !string.IsNullOrEmpty(certContent));
            Console.WriteLine("Private key content available: " + !string.IsNullOrEmpty(privateKeyContent));


            Console.WriteLine(certContent);
            Console.WriteLine(privateKeyContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}