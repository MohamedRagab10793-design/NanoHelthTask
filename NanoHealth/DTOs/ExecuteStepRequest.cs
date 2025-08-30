namespace NanoHealth.DTOs;

public class ExecuteStepRequest
{
    public int ProcessId { get; set; }
    public string StepName { get; set; } = string.Empty;
    public string PerformedBy { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string? Comments { get; set; }
}
