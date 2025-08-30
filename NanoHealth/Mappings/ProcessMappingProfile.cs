using AutoMapper;
using NanoHealth.Data.Entities;
using NanoHealth.DTOs;

namespace NanoHealth.Mappings;

public class ProcessMappingProfile : Profile
{
    public ProcessMappingProfile()
    {
  

        CreateMap<Process, ProcessListResponse>()
            .ForMember(dest => dest.WorkflowName, opt => opt.MapFrom(src => src.Workflow.Name));

        CreateMap<StartProcessRequest, Process>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.StartedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CompletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.CurrentStep, opt => opt.Ignore())
            .ForMember(dest => dest.Workflow, opt => opt.Ignore())
            .ForMember(dest => dest.Executions, opt => opt.Ignore());

        CreateMap<ExecuteStepRequest, ProcessExecution>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ExecutedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ValidationPassed, opt => opt.Ignore())
            .ForMember(dest => dest.ValidationError, opt => opt.Ignore())
            .ForMember(dest => dest.Process, opt => opt.Ignore());
    }
}
