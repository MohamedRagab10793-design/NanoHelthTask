namespace NanoHealth.DTOs;

public class WorkflowStepResponse
{
    public int Id { get; set; }
    public string StepName { get; set; } = string.Empty;
    public string AssignedTo { get; set; } = string.Empty;
    public string ActionType { get; set; } = string.Empty;
    public string? NextStep { get; set; }
    public int Order { get; set; }
}
