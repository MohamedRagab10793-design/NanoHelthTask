namespace NanoHealth.DTOs;

public class WorkflowResponse
{
    public int Id { get; set; }
    public string Name { get; set; } 
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public List<WorkflowStepResponse> Steps { get; set; } = new List<WorkflowStepResponse>();
}
