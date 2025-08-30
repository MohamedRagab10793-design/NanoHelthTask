using NanoHealth.Enums;

namespace NanoHealth.DTOs;

public class ProcessListResponse
{
    public int Id { get; set; }
    public int WorkflowId { get; set; }
    public string WorkflowName { get; set; } = string.Empty;
    public string Initiator { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public ProcessStatus Status { get; set; }
    public string? CurrentStep { get; set; }
}
