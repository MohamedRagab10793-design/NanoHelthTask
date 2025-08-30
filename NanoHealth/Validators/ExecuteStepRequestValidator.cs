using FluentValidation;
using NanoHealth.DTOs;

namespace NanoHealth.Validators;

public class ExecuteStepRequestValidator : AbstractValidator<ExecuteStepRequest>
{
    public ExecuteStepRequestValidator()
    {
        RuleFor(x => x.ProcessId)
            .GreaterThan(0).WithMessage("Process ID must be greater than 0");

        RuleFor(x => x.StepName)
            .NotEmpty().WithMessage("Step name is required")
            .MaximumLength(100).WithMessage("Step name cannot exceed 100 characters");

        RuleFor(x => x.PerformedBy)
            .NotEmpty().WithMessage("PerformedBy is required")
            .MaximumLength(50).WithMessage("PerformedBy cannot exceed 50 characters");

        RuleFor(x => x.Action)
            .NotEmpty().WithMessage("Action is required")
            .MaximumLength(50).WithMessage("Action cannot exceed 50 characters")
            .Must(action => new[] { "approve", "reject", "complete", "input" }.Contains(action.ToLower()))
            .WithMessage("Action must be one of: approve, reject, complete, input");

        RuleFor(x => x.Comments)
            .MaximumLength(500).WithMessage("Comments cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Comments));
    }
}
