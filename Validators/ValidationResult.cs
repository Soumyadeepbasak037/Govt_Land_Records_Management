using static System.Runtime.InteropServices.JavaScript.JSType;

namespace govt_land_service.Validators
{
    public class ValidationResult
    {
        public bool IsValid { get; }
        public string ErrorMessage { get; }


        private ValidationResult(bool isValid, string errorMessage)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        public static ValidationResult Success() =>
            new ValidationResult(true, string.Empty);

        public static ValidationResult Failure(string errorMessage) =>
            new ValidationResult(false, errorMessage);
    }
}
