using AutoMapper;
using SafeCity.Domain.Entity;
using SafeCity.DTOs.Case;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Case, CaseResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                src.Status.ToString().Replace("_", " ")
            ));
    }
}