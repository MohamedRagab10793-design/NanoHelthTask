using NanoHealth.Enums;

namespace NanoHealth.DTOs
{
    public class ProcessFilterRequest
    {
        public int? WorkflowId { get; set; }
        public ProcessStatus? Status { get; set; }
        public string? AssignedTo { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
