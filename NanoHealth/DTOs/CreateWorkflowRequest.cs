namespace NanoHealth.DTOs;

public class CreateWorkflowRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<WorkflowStepRequest> Steps { get; set; } = new List<WorkflowStepRequest>();
}
