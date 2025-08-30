using Microsoft.EntityFrameworkCore;
using NanoHealth.Data;
using NanoHealth.Data.Entities;
using NanoHealth.DTOs;
using NanoHealth.Enums;
using AutoMapper;

namespace NanoHealth.Services;

public class ProcessService : IProcessService
{
    private readonly WorkflowDbContext _context;
    private readonly IValidationService _validationService;
    private readonly ILogger<ProcessService> _logger;
    private readonly IMapper _mapper;

    public ProcessService(WorkflowDbContext context, IValidationService validationService, ILogger<ProcessService> logger, IMapper mapper)
    {
        _context = context;
        _validationService = validationService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<bool> StartProcessAsync(StartProcessRequest request)
    {
        var workflow = await _context.Workflows.AsNoTracking()
                      .Include(w => w.Steps.OrderBy(s => s.Order))
                      .FirstOrDefaultAsync(w => w.Id == request.WorkflowId);

        if (workflow == null)
            throw new ArgumentException("Workflow not found or inactive");

        var process = _mapper.Map<Process>(request);
        process.StartedAt = DateTime.UtcNow;
        process.Status = ProcessStatus.InProgress;
        process.CurrentStep = workflow.Steps.FirstOrDefault()?.StepName;
        _context.Processes.Add(process);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Started process {ProcessId} for workflow {WorkflowName} by {Initiator}",
            process.Id, workflow.Name, request.Initiator);
        return true;
    }

    public async Task<bool> ExecuteStepAsync(ExecuteStepRequest request)
    {
        var process = await _context.Processes
                     .Include(p => p.Workflow).ThenInclude(w => w.Steps.OrderBy(s => s.Order))
                     .Include(p => p.Executions)
                     .FirstOrDefaultAsync(p => p.Id == request.ProcessId);

        if (process == null)
            throw new ArgumentException("Process not found");

        if (process.Status == ProcessStatus.Completed)
            throw new InvalidOperationException("Cannot execute step on completed process");

        var currentStep = process.Workflow.Steps.FirstOrDefault(s => s.StepName == request.StepName);
        if (currentStep == null)
            throw new ArgumentException("Step not found in workflow");

        if (currentStep.AssignedTo != request.PerformedBy)
            throw new UnauthorizedAccessException($"User {request.PerformedBy} is not authorized for step {request.StepName}");

        // Validate step if required
        bool validationPassed = true;
        string? validationError = null;
        bool needsValidation = true;
        if (needsValidation)
        {
            var validationResult = await _validationService.ValidateStepAsync(
                currentStep.StepName,
                 "",
                null);

            validationPassed = validationResult.IsValid;
            validationError = validationResult.ErrorMessage;

            if (!validationPassed)
            {
                // Log the validation failure
                _logger.LogWarning("Validation failed for step {StepName} in process {ProcessId}: {Error}",
                    request.StepName, request.ProcessId, validationError);

                // Create execution record for failed validation
                var execution = _mapper.Map<ProcessExecution>(request);
                execution.ExecutedAt = DateTime.UtcNow;
                execution.ValidationPassed = false;
                execution.ValidationError = validationError;

                _context.ProcessExecutions.Add(execution);

                // Update process status to failed
                process.Status = ProcessStatus.Failed;

                await _context.SaveChangesAsync();

                throw new InvalidOperationException($"Validation failed: {validationError}");
            }
        }

        // Create execution record
        var processExecution = _mapper.Map<ProcessExecution>(request);
        processExecution.ExecutedAt = DateTime.UtcNow;
        processExecution.ValidationPassed = validationPassed;
        processExecution.ValidationError = validationError;

        _context.ProcessExecutions.Add(processExecution);

        var nextStep = GetNextStep(process.Workflow.Steps, currentStep, request.Action);

        if (nextStep == null)
        {
            process.Status = ProcessStatus.Completed;
            process.CompletedAt = DateTime.UtcNow;
            process.CurrentStep = null;
        }
        else
        {
            process.CurrentStep = nextStep.StepName;
        }
        await _context.SaveChangesAsync();

        _logger.LogInformation("Executed step {StepName} in process {ProcessId} by {PerformedBy}",
            request.StepName, request.ProcessId, request.PerformedBy);

        return true;
    }


    public async Task<PaginatedResponse<ProcessListResponse>> GetProcessesAsync(ProcessFilterRequest filter)
    {
        filter.PageSize = Math.Max(1, Math.Min(100, filter.PageSize));
        var query = _context.Processes.Include(p => p.Workflow).AsQueryable();
        if (filter.WorkflowId.HasValue)
            query = query.Where(p => p.WorkflowId == filter.WorkflowId.Value);

        if (filter.Status.HasValue)
            query = query.Where(p => p.Status == filter.Status.Value);

        if (!string.IsNullOrEmpty(filter.AssignedTo))
        {
            query = query.Where(p => p.CurrentStep != null &&
                p.Workflow.Steps.Any(s => s.StepName == p.CurrentStep && s.AssignedTo == filter.AssignedTo));
        }

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);
        var items = await _mapper.ProjectTo<ProcessListResponse>(query
                                                .Skip((filter.PageNumber - 1) * filter.PageSize)
                                                .Take(filter.PageSize))
                                                .ToListAsync();

        return new PaginatedResponse<ProcessListResponse>
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

    private WorkflowStep? GetNextStep(IEnumerable<WorkflowStep> steps, WorkflowStep currentStep, string action)
    {
        if (action.Equals("reject", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (!string.IsNullOrEmpty(currentStep.NextStep))
        {
            return steps.FirstOrDefault(s => s.StepName == currentStep.NextStep);
        }

        var nextStepByOrder = steps.FirstOrDefault(s => s.Order == currentStep.Order + 1);
        return nextStepByOrder;
    }
}
