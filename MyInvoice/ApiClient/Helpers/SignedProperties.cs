namespace MyInvois.ApiClient.Helpers
{
    internal class UBLExtension(string invoiceHash, string signedPropertiesHash, string signatureValue, string certificateContent, string signatureTimestamp, string publicKeyHashing, string SupplierName, string serialNumber)
    {
        public string InvoiceHash { get; set; } = invoiceHash;
        public string SignedPropertiesHash { get; set; } = signedPropertiesHash;
        public string SignatureValue { get; set; } = signatureValue;
        public string CertificateContent { get; set; } = certificateContent;
        public string SignatureTimestamp { get; set; } = signatureTimestamp;
        public string PublicKeyHashing { get; set; } = publicKeyHashing;
        public string SupplierName { get; set; } = SupplierName;
        public string SerialNumber { get; set; } = serialNumber;

        public override string ToString()
        {
            string stringUBLExtension = @"resources\UBLExtensionTemplate.txt";
            stringUBLExtension = stringUBLExtension.
                Replace("INVOICE_HASH", InvoiceHash).
                Replace("SIGNED_PROPERTIES", SignedPropertiesHash).
                Replace("SIGNATURE_VALUE", SignatureValue).
                Replace("CERTIFICATE_CONTENT", CertificateContent).
                Replace("SIGNATURE_TIMESTAMP", SignatureTimestamp).
                Replace("PUBLICKEY_HASHING", PublicKeyHashing).
                Replace("Supplier_NAME", SupplierName).
                Replace("SERIAL_NUMBER", SerialNumber);

            return stringUBLExtension;
        }
    }
}
