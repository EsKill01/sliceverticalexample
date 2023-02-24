namespace TP.NA.CartService.Application.Exceptions
{
    using FluentValidation.Results;

    /// <summary>
    /// Custom forbidden access exception
    /// </summary>
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() : base()
        {
        }
    }

    /// <summary>
    /// Custom not found exception
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException() : base()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NotFoundException(string name, object key) : base($"Property\"{name}\"({key}) was nor found.")
        {
        }
    }

    /// <summary>
    /// Custom validation exception
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(List<ValidationFailure> failures) : base(BuildErrorMessage(failures))
        {
        }

        public IDictionary<string, string[]> failures = new Dictionary<string, string[]>();

        private static string BuildErrorMessage(IEnumerable<ValidationFailure> errors)
        {
            IEnumerable<string> values = errors.Select((x) => " " + x.PropertyName + ": " + x.ErrorMessage);
            return "Validation failed: " + string.Join(string.Empty, values);
        }
    }
}