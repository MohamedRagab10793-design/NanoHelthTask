using Microsoft.EntityFrameworkCore;
using NanoHealth.Data;
using NanoHealth.Data.Entities;
using NanoHealth.DTOs;
using AutoMapper;

namespace NanoHealth.Services;

public class WorkflowService : IWorkflowService
{
    private readonly WorkflowDbContext _context;
    private readonly ILogger<WorkflowService> _logger;
    private readonly IMapper _mapper;

    public WorkflowService(WorkflowDbContext context, ILogger<WorkflowService> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<WorkflowResponse> CreateAsync(CreateWorkflowRequest request)
    {
        var workflow = _mapper.Map<Workflow>(request);
        workflow.CreatedAt = DateTime.UtcNow;
        for (int i = 0; i < request.Steps.Count; i++)
        {
            var stepRequest = request.Steps[i];
            var step = _mapper.Map<WorkflowStep>(stepRequest);
            step.Order = i + 1;
            step.WorkflowId = workflow.Id;
            workflow.Steps.Add(step);   
        }
        _context.Workflows.Add(workflow);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created workflow: {WorkflowName} with {StepCount} steps", workflow.Name, request.Steps.Count);
        return _mapper.Map<WorkflowResponse>(workflow);
    }

    public async Task<WorkflowResponse> UpdateAsync(int id, CreateWorkflowRequest request)
    {
        var workflow = await _context.Workflows.Include(w => w.Steps).FirstOrDefaultAsync(w => w.Id == id);

        if (workflow == null) throw new ArgumentException("Workflow not found");

        workflow.Name = request.Name;
        workflow.Description = request.Description;

        _context.WorkflowSteps.RemoveRange(workflow.Steps);
        for (int i = 0; i < request.Steps.Count; i++)
        {
            var stepRequest = request.Steps[i];
            var step = _mapper.Map<WorkflowStep>(stepRequest);
            step.Order = i + 1;
            step.WorkflowId = workflow.Id;
            _context.WorkflowSteps.Add(step);
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated workflow: {WorkflowName}", workflow.Name);
        return _mapper.Map<WorkflowResponse>(workflow);
    }

    public async Task<WorkflowResponse> Get(int id)
    {
        return await _mapper.ProjectTo<WorkflowResponse>(_context.Workflows
                            .Include(w => w.Steps.OrderBy(s => s.Order))).FirstOrDefaultAsync() ?? throw new ArgumentException("Workflow not found");
    }
    public async Task<PaginatedResponse<WorkflowListResponse>> Get(WorkFlowFilterRequest filter)
    {
        filter.PageSize = Math.Max(1, Math.Min(100, filter.PageSize));

        var query = _context.Workflows.AsNoTracking().OrderByDescending(w => w.CreatedAt).AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(w => w.Name.Contains(filter.Name));

        var totalCount = await query.CountAsync();

        var items = await _mapper.ProjectTo<WorkflowListResponse>(query
                                                .Skip((filter.PageNumber - 1) * filter.PageSize)
                                                .Take(filter.PageSize))
                                                .ToListAsync();

        var totalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize);

        return new PaginatedResponse<WorkflowListResponse>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalPages = totalPages,
            HasPreviousPage = filter.PageNumber > 1,
            HasNextPage = filter.PageNumber < totalPages
        };
    }
}
