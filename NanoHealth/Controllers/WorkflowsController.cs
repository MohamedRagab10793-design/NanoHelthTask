using Microsoft.AspNetCore.Mvc;
using NanoHealth.DTOs;
using NanoHealth.Services;

namespace NanoHealth.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class WorkflowsController(IWorkflowService workflowService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<WorkflowResponse>> CreateWorkflow([FromBody] CreateWorkflowRequest request)
        => await workflowService.CreateAsync(request);

    [HttpPut("{id}")]
    public async Task<ActionResult<WorkflowResponse>> UpdateWorkflow([FromRoute] int id, [FromBody] CreateWorkflowRequest request)
        => await workflowService.UpdateAsync(id, request);

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkflowResponse>> Get([FromRoute] int id)
        => await workflowService.Get(id);

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<WorkflowListResponse>>> Get([FromQuery] WorkFlowFilterRequest filter)
        => await workflowService.Get(filter);
}
