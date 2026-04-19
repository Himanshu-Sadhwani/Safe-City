using AutoMapper;
using SafeCity.Domain.Entity;
using SafeCity.DTOs.Case;
using SafeCity.DTOs;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // case response mapping
        CreateMap<Case, CaseResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                src.Status.ToString().Replace("_", " ")
            ));

        CreateMap<CaseCreation, Case>().ForMember(dest => dest.ResolutionDate, opt => opt.MapFrom(src => DateTime.Now)).ForMember(dest => dest.CaseID, opt => opt.Ignore());

        // compliance mappings
        CreateMap<CreateComplianceRequestDto, ComplianceRecord>()
            .ForMember(dest => dest.EntityID, opt => opt.MapFrom(src => src.EntityId))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ComplianceID, opt => opt.Ignore());

        CreateMap<ComplianceRecord, CreateComplianceResponseDto>();
    }
}