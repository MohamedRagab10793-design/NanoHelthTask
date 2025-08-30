namespace NanoHealth.DTOs;

public class StartProcessRequest
{
    public int WorkflowId { get; set; }
    public string Initiator { get; set; } = string.Empty;
}
