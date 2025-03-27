$InformationPreference = "SilentlyContinue"
function Minify-Json {
    param (
        [Parameter(Mandatory = $true)]
        [string]$JsonString
    )
    # Validate minified JSON content
    if ($JsonString -eq $null -or $JsonString -eq '') {
        Write-Host "Invalid JSON content."
        exit
    }
    # Remove all whitespace between elements, preserving whitespace within string values
    $minifiedJson = [System.Text.RegularExpressions.Regex]::Replace($JsonString, '("(?:\\.|[^"\\])*")|\s+', {
            param($match)
            if ($match.Groups[1].Success) {
                return $match.Groups[1].Value
            }
            else {
                return ''
            }
        })
    return $minifiedJson
}
Clear-Host

$chkPath = "..\..\cert\cert_dev.p12"
$filePath = "..\..\json_files\input.json"
$fileOutPath = "..\..\json_files\output.json"
$encodedJsonPath = "..\..\json_files\debug\encoded.json"

# Read Certificate Password
$password = Read-Host -AsSecureString -Prompt "Enter the password for your certificate file"

# Set API Code Number
$CodeNumber = Read-Host -Prompt "Enter the API Code Number"

Clear-Host
# Loop until a valid file is found or 'X' is entered
while (-not (Test-Path $filePath)) {
    # Prompt the user to enter the path of the file or 'X' to exit
    Clear-Host
    $filePath = Read-Host -Prompt "File not found. Enter the path of the payload file or 'X' to exit"
    # Check if the file exists
    if (-not (Test-Path $filePath)) {
        # Check if the user wants to exit
        if ($filePath -eq "X") {
            Clear-Host
            exit
        }
        if ($filePath -eq "x") {
            Clear-Host
            exit
        }
        Clear-Host
        Write-Host "File not found: $filePath"
    }
}
# Read the content of the file
$document = Get-Content -Path $filePath -Raw
# Load necessary assemblies
Add-Type -AssemblyName System.Security
# Minify the JSON string
$minifiedJson = Minify-Json -JsonString $document
# Write the minified json data to the file
$minifiedJson | Out-File -FilePath $fileOutPath -Encoding UTF8
# Create a new instance of SHA256
$sha256 = [System.Security.Cryptography.SHA256]::Create()
# Compute the hash of the document
$hash = $sha256.ComputeHash([System.Text.Encoding]::UTF8.GetBytes($minifiedJson))
# Convert the hash to a Base64 string
$DocDigest = [System.Convert]::ToBase64String($hash)
# Load necessary assemblies
Add-Type -AssemblyName System.Security
Clear-Host
# Loop until a valid file is found or 'X' is entered
while (-not (Test-Path $chkPath)) {
    # Prompt the user to enter the path of the file or 'X' to exit
    Clear-Host
    $chkPath = Read-Host -Prompt "File not found. Enter the path of your certificate file (*.pfx/*.p12) or 'X' to exit"
    # Check if the file exists
    if (-not (Test-Path $chkPath)) {
        # Check if the user wants to exit
        if ($chkPath -eq "X") {
            Clear-Host
            break
        }
        if ($chkPath -eq "x") {
            Clear-Host
            break
        }
        Clear-Host
        Write-Host "File not found: $chkPath"
    }
} #while loop
if (-not (Test-Path $chkPath)) {
    Clear-Host
    Write-Host "File not found: $chkPath"
}
else {
    $certPath = $chkPath
    # Remove the variable to free up memory
    Remove-Variable -Name $chkPath
    Clear-Host
    try { $cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2($certPath, $password) } catch {
        Write-Host "Failed to load
certificate: $_" exit 
    }
    # Remove the variable to free up memory
    Remove-Variable -Name $certPath
    # Extract the private key using RSACryptoServiceProvider
    $privateKeyProvider = [System.Security.Cryptography.X509Certificates.RSACertificateExtensions]::GetRSAPrivateKey($cert)
    # Create RSA signature formatter
    $rsaFormatter = New-Object System.Security.Cryptography.RSAPKCS1SignatureFormatter $privateKeyProvider
    $rsaFormatter.SetHashAlgorithm("SHA256")
    # Sign the hash
    $signature = $rsaFormatter.CreateSignature($hash)
    # Convert the signature to Base64
    $signatureBase64 = [Convert]::ToBase64String($signature)
}
Clear-Host
# Compute the hash of the certificate using SHA-256
$sha256 = [System.Security.Cryptography.SHA256]::Create()
$certHash = $sha256.ComputeHash($cert.RawData)
# Convert the hash to a Base64 encoded string
$certDigest = [Convert]::ToBase64String($certHash)
# Ensure data sanitization
$issuerName = [System.Security.SecurityElement]::Escape($cert.IssuerName.Name)
# Retrieve the serial number
$serialNumber = $cert.SerialNumber
# Convert the hexadecimal string to a BigInteger
$CertSerialNumber = [System.Numerics.BigInteger]::Parse($serialNumber, [System.Globalization.NumberStyles]::HexNumber)
# Extract the raw data of the certificate
$rawData = [Convert]::ToBase64String($cert.RawData)
#Set SigningTime to current timestamp based on UTC format
#$utcTimestamp = Get-Date -Format "yyyy-MM-ddTHH:mm:ssZ"
$utcTimestamp = "2024-11-25T07:52:32Z"
Clear-Host
# Assign the XML string to a variable using a here-string
$jsonString = @"
{"Target":"signature","SignedProperties":[{"Id":"id-xades-signed-props","SignedSignatureProperties":[{"SigningTime":[{"_":"$utcTimestamp"}],"SigningCertificate":[{"Cert":[{"CertDigest":[{"DigestMethod":[{"_":"","Algorithm":"http://www.w3.org/2001/04/xmlenc#sha256"}],"DigestValue":[{"_":"$certDigest"}]}],"IssuerSerial":[{"X509IssuerName":[{"_":"$issuerName"}],"X509SerialNumber":[{"_":"$CertSerialNumber"}]}]}]}]}]}]}
"@
# Minify the JSON string
$signedprops = Minify-Json -JsonString $jsonString
# Compute the hash of the signedprops' UTF-8 bytes
$signedpropshash = $sha256.ComputeHash([System.Text.Encoding]::UTF8.GetBytes($signedprops))
# Convert the hash to a Base64 encoded string
$signedpropsdigest = [Convert]::ToBase64String($signedpropshash)
# Retrieve the subject name from the certificate
$subjectName = $cert.SubjectName.Name
$signString = @"
}],"UBLExtensions":[{"UBLExtension":[{"ExtensionURI":[{"_":"urn:oasis:names:specification:ubl:dsig:enveloped:xades"}],"ExtensionContent":[{"UBLDocumentSignatures":[{"SignatureInformation":[{"ID":[{"_":"urn:oasis:names:specification:ubl:signature:1"}],"ReferencedSignatureID":[{"_":"urn:oasis:names:specification:ubl:signature:Invoice"}],"Signature":[{"Id":"signature","Object":[{"QualifyingProperties":[{"Target":"signature","SignedProperties":[{"Id":"id-xades-signed-props","SignedSignatureProperties":[{"SigningTime":[{"_":"$utcTimestamp"}],"SigningCertificate":[{"Cert":[{"CertDigest":[{"DigestMethod":[{"_":"","Algorithm":"http://www.w3.org/2001/04/xmlenc#sha256"}],"DigestValue":[{"_":"$certDigest"}]}],"IssuerSerial":[{"X509IssuerName":[{"_":"$issuerName"}],"X509SerialNumber":[{"_":"$CertSerialNumber"}]}]}]}]}]}]}]}],"KeyInfo":[{"X509Data":[{"X509Certificate":[{"_":"$rawData"}],"X509SubjectName":[{"_":"$subjectName"}],"X509IssuerSerial":[{"X509IssuerName":[{"_":"$issuerName"}],"X509SerialNumber":[{"_":"$CertSerialNumber"}]}]}]}],"SignatureValue":[{"_":"$signatureBase64"}],"SignedInfo":[{"SignatureMethod":[{"_":"","Algorithm":"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"}],"Reference":[{"Type":"http://uri.etsi.org/01903/v1.3.2#SignedProperties","URI":"#id-xades-signed-props","DigestMethod":[{"_":"","Algorithm":"http://www.w3.org/2001/04/xmlenc#sha256"}],"DigestValue":[{"_":"$signedpropsdigest"}]},{"Type":"","URI":"","DigestMethod":[{"_":"","Algorithm":"http://www.w3.org/2001/04/xmlenc#sha256"}],"DigestValue":[{"_":"$DocDigest"}]}]}]}]}]}]}]}]}],"Signature":[{"ID":[{"_":"urn:oasis:names:specification:ubl:signature:Invoice"}],"SignatureMethod":[{"_":"urn:oasis:names:specification:ubl:dsig:enveloped:xades"}]}]}]}
"@
# Remove the variable to free up memory
Remove-Variable -Name $cert
# Read the content of JsonFile1
$jsonFile1Content = Get-Content -Path $fileOutPath -Raw
# Remove the last 3 characters from the file content
$modifiedContent = $jsonFile1Content.Substring(0, $jsonFile1Content.Length - 7)
# Combine the data from JsonFile1 and JsonFile2
$combinedData = $modifiedContent + $signString
# Minify the JSON string
$finaldata = Minify-Json -JsonString $combinedData
# Write the updated content back to the file
$finaldata | Out-File -FilePath $fileOutPath -Encoding UTF8
# Encode the content using Base64
$encodedContent = [System.Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes($finaldata))
# Write the encoded content to a new file
$encodedContent | Out-File -FilePath $encodedJsonPath -Encoding UTF8
# Create a SHA-256 hash object
$sha256 = [System.Security.Cryptography.SHA256]::Create()
# Compute the hash of the document
$hashString = $sha256.ComputeHash([System.Text.Encoding]::UTF8.GetBytes($finaldata))
# Convert the hash bytes to a hexadecimal string
$Digest = $hashString | ForEach-Object { $_.ToString("x2") }
# Output the encoded content
$RecHash = $Digest -join "" 
$JSONApi = @"
{"documents":[{"format":"JSON","documentHash":"$RecHash","codeNumber":"$CodeNumber","document":"$encodedContent"}]}
"@
Clear-Host
$JSONApi
# Remove the variable to free up memory
Remove-Variable -Name $JSONApi