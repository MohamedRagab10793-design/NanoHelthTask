using FluentValidation;
using NanoHealth.DTOs;

namespace NanoHealth.Validators;

public class StartProcessRequestValidator : AbstractValidator<StartProcessRequest>
{
    public StartProcessRequestValidator()
    {
        RuleFor(x => x.WorkflowId)
            .GreaterThan(0).WithMessage("Workflow id must be greater than 0");

        RuleFor(x => x.Initiator)
            .NotEmpty().WithMessage("Initiator is required")
            .MaximumLength(50).WithMessage("Initiator cannot exceed 50 characters");
    }
}
