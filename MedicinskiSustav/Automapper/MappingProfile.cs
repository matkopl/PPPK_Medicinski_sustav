using AutoMapper;
using MedicinskiSustav.Models;
using MedicinskiSustav.Viewmodels;

namespace MedicinskiSustav.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PacijentCreateVM, Pacijent>();
            CreateMap<PacijentEditVM, Pacijent>().ReverseMap();
            CreateMap<Pacijent, PacijentDetailsVM>()
                .ForMember(dest => dest.Bolesti, opt => opt.MapFrom(src => src.Dokumentacija.Bolesti))
                .ForMember(dest => dest.Pregledi, opt => opt.MapFrom(src => src.Pregledi));
        }
    }
}
