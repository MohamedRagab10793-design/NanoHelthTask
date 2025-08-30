using FluentValidation;
using NanoHealth.DTOs;

namespace NanoHealth.Validators;

public class CreateWorkflowRequestValidator : AbstractValidator<CreateWorkflowRequest>
{
    public CreateWorkflowRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Workflow name is required")
            .MaximumLength(100).WithMessage("Workflow name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Steps)
            .NotEmpty().WithMessage("At least one step is required")
            .Must(steps => steps != null && steps.Count > 0).WithMessage("Steps list cannot be empty");

        RuleForEach(x => x.Steps).SetValidator(new WorkflowStepRequestValidator());
    }
}
