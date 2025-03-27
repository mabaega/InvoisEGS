using InvoisEGS.ApiClient.XModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;

namespace InvoisEGS.ApiClient.ApiHelpers
{
    public static class JsonUtility
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static string GenerateSignString(string utcTimestamp, string certDigest, string issuerName,
            string certSerialNumber, string rawData, string subjectName, string signatureBase64,
            string signedPropsDigest, string documentDigestBase64)
        {
            string template = ApiHelpers.XmlJsonUtilities.LoadEmbededResources("InvoisEGS.ApiClient.Resources.JsonUBLExtensions.txt");

            template = template
                .Replace("${UTC_TIMESTAMP}", utcTimestamp)
                .Replace("${CERT_DIGEST}", certDigest)
                .Replace("${ISSUER_NAME}", issuerName)
                .Replace("${CERT_SERIAL_NUMBER}", certSerialNumber)
                .Replace("${RAW_DATA}", rawData)
                .Replace("${SUBJECT_NAME}", subjectName)
                .Replace("${SIGNATURE_BASE64}", signatureBase64)
                .Replace("${SIGNED_PROPS_DIGEST}", signedPropsDigest)
                .Replace("${DOCUMENT_DIGEST_BASE64}", documentDigestBase64);

            return JsonUtility.MinifyJson(template).TrimStart('{').TrimEnd('}');
        } 
        public static string GenerateSignedPropertiesJson(string utcTimestamp, string certDigest, string issuerName, string certSerialNumber)
        {
            string template = ApiHelpers.XmlJsonUtilities.LoadEmbededResources("InvoisEGS.ApiClient.Resources.JsonSignedProps.txt");

            template = template
                .Replace("${UTC_TIMESTAMP}", utcTimestamp)
                .Replace("${CERT_DIGEST}", certDigest)
                .Replace("${ISSUER_NAME}", issuerName)
                .Replace("${CERT_SERIAL_NUMBER}", certSerialNumber);

            return template;
        }

        /* public static string GenerateSignString(string utcTimestamp, string certDigest, string issuerName,
            string certSerialNumber, string rawData, string subjectName, string signatureBase64,
            string signedPropsDigest, string documentDigestBase64)
        {
            return $@"}}],""UBLExtensions"":[{{""UBLExtension"":[{{""ExtensionURI"":[{{""_"":""urn:oasis:names:specification:ubl:dsig:enveloped:xades""}}],""ExtensionContent"":[{{""UBLDocumentSignatures"":[{{""SignatureInformation"":[{{""ID"":[{{""_"":""urn:oasis:names:specification:ubl:signature:1""}}],""ReferencedSignatureID"":[{{""_"":""urn:oasis:names:specification:ubl:signature:Invoice""}}],""Signature"":[{{""Id"":""signature"",""Object"":[{{""QualifyingProperties"":[{{""Target"":""signature"",""SignedProperties"":[{{""Id"":""id-xades-signed-props"",""SignedSignatureProperties"":[{{""SigningTime"":[{{""_"":""{utcTimestamp}""}}],""SigningCertificate"":[{{""Cert"":[{{""CertDigest"":[{{""DigestMethod"":[{{""_"":"""",""Algorithm"":""http://www.w3.org/2001/04/xmlenc#sha256""}}],""DigestValue"":[{{""_"":""{certDigest}""}}]}}],""IssuerSerial"":[{{""X509IssuerName"":[{{""_"":""{issuerName}""}}],""X509SerialNumber"":[{{""_"":""{certSerialNumber}""}}]}}]}}]}}]}}]}}]}}]}}],""KeyInfo"":[{{""X509Data"":[{{""X509Certificate"":[{{""_"":""{rawData}""}}],""X509SubjectName"":[{{""_"":""{subjectName}""}}],""X509IssuerSerial"":[{{""X509IssuerName"":[{{""_"":""{issuerName}""}}],""X509SerialNumber"":[{{""_"":""{certSerialNumber}""}}]}}]}}]}}],""SignatureValue"":[{{""_"":""{signatureBase64}""}}],""SignedInfo"":[{{""SignatureMethod"":[{{""_"":"""",""Algorithm"":""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256""}}],""Reference"":[{{""Type"":""http://uri.etsi.org/01903/v1.3.2#SignedProperties"",""URI"":""#id-xades-signed-props"",""DigestMethod"":[{{""_"":"""",""Algorithm"":""http://www.w3.org/2001/04/xmlenc#sha256""}}],""DigestValue"":[{{""_"":""{signedPropsDigest}""}}]}},{{""Type"":"""",""URI"":"""",""DigestMethod"":[{{""_"":"""",""Algorithm"":""http://www.w3.org/2001/04/xmlenc#sha256""}}],""DigestValue"":[{{""_"":""{documentDigestBase64}""}}]}}]}}]}}]}}]}}]}}]}}],""Signature"":[{{""ID"":[{{""_"":""urn:oasis:names:specification:ubl:signature:Invoice""}}],""SignatureMethod"":[{{""_"":""urn:oasis:names:specification:ubl:dsig:enveloped:xades""}}]}}]}}]}}";
        }
        public static string GenerateSignedPropertiesJson(string utcTimestamp, string certDigest, string issuerName, string certSerialNumber)
        {
            return $@"{{""Target"":""signature"",""SignedProperties"":[{{""Id"":""id-xades-signed-props"",""SignedSignatureProperties"":[{{""SigningTime"":[{{""_"":""{utcTimestamp}""}}],""SigningCertificate"":[{{""Cert"":[{{""CertDigest"":[{{""DigestMethod"":[{{""_"":"""",""Algorithm"":""http://www.w3.org/2001/04/xmlenc#sha256""}}],""DigestValue"":[{{""_"":""{certDigest}""}}]}}],""IssuerSerial"":[{{""X509IssuerName"":[{{""_"":""{issuerName}""}}],""X509SerialNumber"":[{{""_"":""{certSerialNumber}""}}]}}]}}]}}]}}]}}]}}";
        } */
        
        public static string MinifyJson(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                throw new ArgumentException("Invalid JSON content.");
            }
            
            string pattern = @"(""(?:\\.|[^""\\])*"")|\s+";
            string minifiedJson = Regex.Replace(jsonString, pattern, m =>
                m.Groups[1].Success ? m.Groups[1].Value : string.Empty);
            
            return minifiedJson;
        }
       
        public static string SerializeToJson(MyInvoice invObject)
        {
            return JsonConvert.SerializeObject(invObject, Settings);
        }

        public static MyInvoice? DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<MyInvoice>(json, Settings);
        }
        public static void SaveToFile(MyInvoice invoice, string filePath)
        {
            string jsonContent = SerializeToJson(invoice);
            File.WriteAllText(filePath, jsonContent, new UTF8Encoding(false));
        }
        public static MyInvoice? LoadFromJsonFile(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            string jsonContent = File.ReadAllText(filePath, Encoding.UTF8);
            return DeserializeFromJson(jsonContent);
        }
        public static string RemoveNullElements(string json)
        {
            var jObject = JObject.Parse(json);
            RemoveEmptyChildren(jObject);
            return JsonConvert.SerializeObject(jObject, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        private static void RemoveEmptyChildren(JToken token)
        {
            if (token is JArray array)
            {
                for (int i = array.Count - 1; i >= 0; i--)
                {
                    RemoveEmptyChildren(array[i]);
                    if (!array[i].Children().Any() || IsEmptyValue(array[i]))
                    {
                        array.RemoveAt(i);
                    }
                }
            }
            else if (token is JObject obj)
            {
                foreach (var prop in obj.Properties().ToList())
                {
                    RemoveEmptyChildren(prop.Value);
                    if (prop.Value.Type == JTokenType.Object && !prop.Value.Children().Any() ||
                        prop.Value.Type == JTokenType.Array && !prop.Value.Children().Any() ||
                        IsEmptyValue(prop.Value))
                    {
                        prop.Remove();
                    }
                }
            }
        }

        private static bool IsEmptyValue(JToken token)
        {
            return token.Type == JTokenType.String && string.IsNullOrEmpty(token.ToString()) ||
                   token.Type == JTokenType.Null;
        }
    }
}
