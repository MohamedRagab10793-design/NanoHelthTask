using Microsoft.AspNetCore.Mvc;
using NanoHealth.DTOs;
using NanoHealth.Services;
using NanoHealth.Enums;

namespace NanoHealth.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class ProcessesController(IProcessService processService) : ControllerBase
{
    [HttpPost("start")]
    public async Task<ActionResult<bool>> StartProcess([FromBody] StartProcessRequest request)
     => await processService.StartProcessAsync(request);


    [HttpPost("execute")]
    public async Task<ActionResult<bool>> ExecuteStep([FromBody] ExecuteStepRequest request)
      => await processService.ExecuteStepAsync(request);


    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<ProcessListResponse>>> GetProcesses([FromQuery] ProcessFilterRequest filter)
       => await processService.GetProcessesAsync(filter);

}
