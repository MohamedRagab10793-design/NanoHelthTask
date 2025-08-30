using NanoHealth.DTOs;

namespace NanoHealth.Services
{
    public interface IWorkflowService
    {
        Task<WorkflowResponse> CreateAsync(CreateWorkflowRequest request);
        Task<WorkflowResponse> UpdateAsync(int id, CreateWorkflowRequest request);
        Task<WorkflowResponse> Get(int id);
        Task<PaginatedResponse<WorkflowListResponse>> Get(WorkFlowFilterRequest filter);
    }
}
