using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace InvoisEGS.Utilities;

public static class CertificateHelper
{

    public static CertificateInfo VerifyPfxCertificate(byte[] certBytes, string? password, string expectedTIN)
    {
        try
        {
            using var certificate = new X509Certificate2(
                certBytes, 
                password,
                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.UserKeySet
            );

            string tin = ExtractTINFromSubject(certificate.Subject);
            if (string.IsNullOrEmpty(tin))
                throw new CryptographicException("TIN not found in certificate subject");
            
            if (!tin.Equals(expectedTIN, StringComparison.OrdinalIgnoreCase))
                throw new CryptographicException($"Certificate TIN ({tin}) does not match expected TIN ({expectedTIN})");

            if (!certificate.HasPrivateKey)
                throw new CryptographicException("Certificate does not contain a private key");

            bool hasNonRepudiation = certificate.Extensions
                .OfType<X509KeyUsageExtension>()
                .Any(x => x.KeyUsages.HasFlag(X509KeyUsageFlags.NonRepudiation));

            if (!hasNonRepudiation)
                throw new CryptographicException("Certificate cannot be used for signing (NonRepudiation flag not present)");

            return ValidateCertificate(certificate, expectedTIN);
        }
        catch (Exception ex)
        {
            throw new CryptographicException($"Failed to verify PFX certificate: {ex.Message}", ex);
        }
    }

    public static (string certificateContent, string privateKeyContent) GetCertificateContents(byte[] certBytes, string? password)
    {
        try
        {
            using var certificate = new X509Certificate2(
                certBytes,
                password,
                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.UserKeySet
            );

            byte[] pfxBytes = certificate.Export(X509ContentType.Pfx);
            string pfxContent = Convert.ToBase64String(pfxBytes);
            
            return (pfxContent, pfxContent);
        }
        catch (Exception ex)
        {
            throw new CryptographicException($"Error extracting certificate contents: {ex.Message}", ex);
        }
    }

    public static CertificateInfo VerifyPemCertificate(byte[] certBytes, string expectedTIN)
    {
        using var cert = new X509Certificate2(certBytes);
        string tin = ExtractTINFromSubject(cert.Subject);
        if (string.IsNullOrEmpty(tin))
            throw new CryptographicException("TIN not found in certificate subject");
            
        if (!tin.Equals(expectedTIN, StringComparison.OrdinalIgnoreCase))
            throw new CryptographicException($"Certificate TIN ({tin}) does not match expected TIN ({expectedTIN})");

        return ValidateCertificate(cert, expectedTIN);
    }

    public static async Task<CertificateInfo> VerifyPemCertificateWithKeyFile(byte[] certBytes, IFormFile privateKeyFile, string expectedTIN)
    {
        using var cert = new X509Certificate2(certBytes);
        string tin = ExtractTINFromSubject(cert.Subject);
        if (string.IsNullOrEmpty(tin))
            throw new CryptographicException("TIN not found in certificate subject");
            
        if (!tin.Equals(expectedTIN, StringComparison.OrdinalIgnoreCase))
            throw new CryptographicException($"Certificate TIN ({tin}) does not match expected TIN ({expectedTIN})");

        using var ms = new MemoryStream();
        await privateKeyFile.CopyToAsync(ms);
        var keyBytes = ms.ToArray();

        using var rsa = RSA.Create();
        var pemKey = System.Text.Encoding.ASCII.GetString(keyBytes);
        rsa.ImportFromPem(pemKey);
        
        var certWithKey = cert.CopyWithPrivateKey(rsa);
        return ValidateCertificate(certWithKey, expectedTIN);
    }

    private static CertificateInfo ValidateCertificate(X509Certificate2 cert, string expectedTIN)
    {
        string tin = ExtractTINFromSubject(cert.Subject);
        bool isTINValid = !string.IsNullOrEmpty(tin) && tin.Equals(expectedTIN, StringComparison.OrdinalIgnoreCase);

        return new CertificateInfo
        {
            Subject = cert.Subject,
            Issuer = cert.Issuer,
            ValidFrom = cert.NotBefore,
            ValidTo = cert.NotAfter,
            SerialNumber = cert.SerialNumber,
            TaxpayerTIN = tin,
            IsValid = DateTime.Now >= cert.NotBefore && 
                     DateTime.Now <= cert.NotAfter && 
                     cert.HasPrivateKey &&
                     isTINValid,
            Certificate = cert
        };
    }

    private static string ExtractTINFromSubject(string subject)
    {
        var match = System.Text.RegularExpressions.Regex.Match(subject, @"OID\.2\.5\.4\.97=([^,]+)");
        return match.Success ? match.Groups[1].Value : string.Empty;
    }
}