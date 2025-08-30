

namespace NanoHealth.Services;

public class ValidationService : IValidationService
{
    private readonly ILogger<ValidationService> _logger;

    public ValidationService(ILogger<ValidationService> logger)
    {
        _logger = logger;
    }

    public async Task<ValidationResult> ValidateStepAsync(string stepName, string validationEndpoint, object? validationData)
    {
        try
        {
            _logger.LogInformation("Validating step: {StepName} with endpoint: {Endpoint}", stepName, validationEndpoint);

            // simulate external api validation

            if (stepName.Contains("Finance", StringComparison.OrdinalIgnoreCase))
            {
                // simulate finance approval validation
                return await SimulateFinanceValidationAsync(validationData);
            }
            else if (stepName.Contains("Manager", StringComparison.OrdinalIgnoreCase))
            {
                // simulate manager approval validation
                return await SimulateManagerValidationAsync(validationData);
            }
            else
            {
                // Default validation - always passes
                return new ValidationResult
                {
                    IsValid = true,
                    ValidationData = validationData
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during validation for step: {StepName}", stepName);
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = $"Validation failed: {ex.Message}"
            };
        }
    }

    private async Task<ValidationResult> SimulateFinanceValidationAsync(object? validationData)
    {
        // simulate api delay
        await Task.Delay(10);

        // simulate finance validation logic
        // In a real scenario, this would call an external financial API
        var random = new Random();
        var isValid = random.Next(1, 10) > 2; // 80% success rate

        if (!isValid)
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = "Financial validation failed: Insufficient budget or credit limit exceeded"
            };
        }

        return new ValidationResult
        {
            IsValid = true,
            ValidationData = new { budgetApproved = true, creditLimit = 10000 }
        };
    }

    private async Task<ValidationResult> SimulateManagerValidationAsync(object? validationData)
    {
        // simulate api delay
        await Task.Delay(50);

        // simulate manager validation logic
        var random = new Random();
        var isValid = random.Next(1, 10) > 1; // 90% success rate

        if (!isValid)
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = "Manager validation failed: Request exceeds approval limits"
            };
        }

        return new ValidationResult
        {
            IsValid = true,
            ValidationData = new { approved = true, approvalLevel = "Manager" }
        };
    }
}

