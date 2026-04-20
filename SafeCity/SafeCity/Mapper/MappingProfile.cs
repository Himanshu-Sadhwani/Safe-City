using AutoMapper;
using SafeCity.Domain.Entity;
using SafeCity.DTOs.Case;
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

    }
}