namespace NanoHealth.Data.Entities;

public class ProcessExecution
{
    public int Id { get; set; }
    public string StepName { get; set; } = string.Empty;
    public string PerformedBy { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
    public string? Comments { get; set; }
    public bool ValidationPassed { get; set; } = true;
    public string? ValidationError { get; set; }

    public int ProcessId { get; set; }
    public virtual Process Process { get; set; } = null!;
}
