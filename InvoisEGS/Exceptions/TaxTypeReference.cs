namespace InvoisEGS.Exceptions
{
    public static class TaxTypeReference
    {
        private static readonly HashSet<string> _TaxType = new()
        {
            "01", "02", "03", "04", "05", "06", "E"
        };
        public static bool ValidateTaxType(string code)
        {
            string normalizedCode = code?.ToUpperInvariant() ?? string.Empty;
            bool isValid = _TaxType.Contains(normalizedCode);
            return isValid;
        }
    }
}