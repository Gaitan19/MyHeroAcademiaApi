using AutoMapper;
using MyHeroAcademiaApi.DTOs.Hero;
using MyHeroAcademiaApi.DTOs.Item;
using MyHeroAcademiaApi.DTOs.Quirk;
using MyHeroAcademiaApi.DTOs.Villain;
using MyHeroAcademiaApi.Models;

namespace MyHeroAcademiaApi.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Hero, HeroDTO>().ReverseMap();
            CreateMap<CreateHeroDTO, Hero>();
            CreateMap<UpdateHeroDTO, Hero>();

            // Configuraciones similares para otras entidades
            CreateMap<Quirk, QuirkDTO>().ReverseMap();
            CreateMap<Villain, VillainDTO>().ReverseMap();
            CreateMap<Item, ItemDTO>().ReverseMap();
        }
    }
}
