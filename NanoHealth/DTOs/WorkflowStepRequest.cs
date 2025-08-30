namespace NanoHealth.DTOs;

public class WorkflowStepRequest
{
    public string StepName { get; set; } = string.Empty;
    public string AssignedTo { get; set; } = string.Empty;
    public string ActionType { get; set; } = string.Empty;
    public string? NextStep { get; set; }
}
