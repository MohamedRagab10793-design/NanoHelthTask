using FluentValidation;
using NanoHealth.DTOs;

namespace NanoHealth.Validators;

public class WorkflowStepRequestValidator : AbstractValidator<WorkflowStepRequest>
{
    public WorkflowStepRequestValidator()
    {
        RuleFor(x => x.StepName)
            .NotEmpty().WithMessage("Step name is required")
            .MaximumLength(100).WithMessage("Step name cannot exceed 100 characters");

        RuleFor(x => x.AssignedTo)
            .NotEmpty().WithMessage("AssignedTo is required")
            .MaximumLength(50).WithMessage("AssignedTo cannot exceed 50 characters");

        RuleFor(x => x.ActionType)
            .NotEmpty().WithMessage("Action type is required")
            .MaximumLength(50).WithMessage("Action type cannot exceed 50 characters")
            .Must(actionType => new[] { "input", "approve_reject", "review", "complete" }.Contains(actionType.ToLower()))
            .WithMessage("Action type must be one of: input, approve_reject, review, complete");

        RuleFor(x => x.NextStep)
            .MaximumLength(100).WithMessage("Next step cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.NextStep));

    }
}
