using AutoMapper;
using NanoHealth.Data.Entities;
using NanoHealth.DTOs;

namespace NanoHealth.Mappings;

public class WorkflowMappingProfile : Profile
{
    public WorkflowMappingProfile()
    {

        CreateMap<Workflow, WorkflowListResponse>();

        CreateMap<Workflow, WorkflowResponse>()
            .ForMember(dest => dest.Steps, opt => opt.MapFrom(src => src.Steps.OrderBy(s => s.Order)));

        CreateMap<WorkflowStep, WorkflowStepResponse>();

        CreateMap<CreateWorkflowRequest, Workflow>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Steps, opt => opt.Ignore())
            .ForMember(dest => dest.Processes, opt => opt.Ignore());

        CreateMap<WorkflowStepRequest, WorkflowStep>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.WorkflowId, opt => opt.Ignore())
            .ForMember(dest => dest.Workflow, opt => opt.Ignore());
    }
}
