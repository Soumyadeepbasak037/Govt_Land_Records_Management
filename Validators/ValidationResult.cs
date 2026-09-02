using static System.Runtime.InteropServices.JavaScript.JSType;

namespace govt_land_service.Validators
{
    public class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<string> Errors { get; } = new();
        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }
}
