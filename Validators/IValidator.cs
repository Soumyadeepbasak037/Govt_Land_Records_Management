namespace govt_land_service.Validators
{
    public interface IValidator
    {
        Task<ValidationResult> ValidateAsync();
    }
}
