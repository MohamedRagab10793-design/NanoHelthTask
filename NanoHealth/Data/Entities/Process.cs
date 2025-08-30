using NanoHealth.Enums;

namespace NanoHealth.Data.Entities;

public class Process
{
    public int Id { get; set; }
    public string Initiator { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public ProcessStatus Status { get; set; } = ProcessStatus.Pending;
    public string? CurrentStep { get; set; }
    public int WorkflowId { get; set; }
    public virtual Workflow Workflow { get; set; } = null!;
    public virtual ICollection<ProcessExecution> Executions { get; set; } = new List<ProcessExecution>();
}
