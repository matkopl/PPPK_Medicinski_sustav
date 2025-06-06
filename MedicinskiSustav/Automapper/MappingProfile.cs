using AutoMapper;
using MedicinskiSustav.Models;
using MedicinskiSustav.Viewmodels;

namespace MedicinskiSustav.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Pacijent, PacijentVM>();
            CreateMap<PacijentCreateVM, Pacijent>();
            CreateMap<PacijentEditVM, Pacijent>().ReverseMap();
            CreateMap<Pacijent, PacijentDetailsVM>()
                .ForMember(dest => dest.Bolesti, opt => opt.MapFrom(src => src.Dokumentacija.Bolesti))
                .ForMember(dest => dest.Pregledi, opt => opt.MapFrom(src => src.Pregledi));

            CreateMap<Bolest, BolestVM>().ReverseMap();
            CreateMap<MedicinskaDokumentacija, DokumentacijaDetailsVM>().ReverseMap();

            CreateMap<Pregled, PregledVM>()
                .ForMember(dest => dest.SifraPregleda, opt => opt.MapFrom(src => src.SifraPregleda.ToString()))
                .ForMember(dest => dest.PacijentImePrezime, opt => opt.MapFrom(src => src.Pacijent.Ime + " " + src.Pacijent.Prezime));
            CreateMap<PregledCreateVM, Pregled>();
            CreateMap<Pregled, PregledDetailsVM>()
                .ForMember(dest => dest.PacijentImePrezime, opt => opt.MapFrom(src => src.Pacijent.Ime + " " + src.Pacijent.Prezime))
                .ForMember(dest => dest.Recepti, opt => opt.MapFrom(src => src.Recepti))
                .ForMember(dest => dest.Slike, opt => opt.MapFrom(src => src.Slike));
            CreateMap<Pregled, PregledEditVM>().ReverseMap();

            CreateMap<Recept, ReceptVM>()
                .ForMember(dest => dest.LijekNaziv, opt => opt.MapFrom(src => src.Lijek.Naziv));
            CreateMap<ReceptCreateVM, Recept>();
            CreateMap<ReceptEditVM, Recept>().ReverseMap();

            CreateMap<Slika, SlikaVM>().ReverseMap();
            CreateMap<SlikaCreateVM, Slika>();
        }
    }
}
