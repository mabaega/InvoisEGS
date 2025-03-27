namespace InvoisEGS.Exceptions
{
    public class ValidationException : Exception
    {
        public List<string> ValidationErrors { get; }

        public ValidationException(string message, List<string> validationErrors) : base(message)
        {
            ValidationErrors = validationErrors ?? new List<string>();
        }

        public ValidationException(string message, List<string> validationErrors, Exception innerException)
            : base(message, innerException)
        {
            ValidationErrors = validationErrors ?? new List<string>();
        }
    }
}