namespace NanoHealth.Services;

public interface IValidationService
{
    Task<ValidationResult> ValidateStepAsync(string stepName, string validationEndpoint, object? validationData);
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public object? ValidationData { get; set; }
}

