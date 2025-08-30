namespace NanoHealth.DTOs;

public class WorkFlowFilterRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string ? Name { get; set; } = null;
}
