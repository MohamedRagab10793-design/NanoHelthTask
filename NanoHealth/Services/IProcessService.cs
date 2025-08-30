using NanoHealth.DTOs;
using NanoHealth.Enums;

namespace NanoHealth.Services;

public interface IProcessService
{
    Task<bool> StartProcessAsync(StartProcessRequest request);
    Task<bool> ExecuteStepAsync(ExecuteStepRequest request);
    Task<PaginatedResponse<ProcessListResponse>> GetProcessesAsync(ProcessFilterRequest filter);
}
