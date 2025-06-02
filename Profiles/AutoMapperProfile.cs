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
            // Mapeo para SimpleQuirkDTO
            CreateMap<Quirk, SimpleQuirkDTO>();

            // Hero mappings
            CreateMap<Hero, HeroDTO>()
                .ForMember(dest => dest.Quirk, opt => opt.MapFrom(src => src.Quirk));

            CreateMap<CreateHeroDTO, Hero>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            CreateMap<UpdateHeroDTO, Hero>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            // Quirk mappings
            CreateMap<Quirk, QuirkDTO>();
            CreateMap<CreateQuirkDTO, Quirk>();
            CreateMap<UpdateQuirkDTO, Quirk>();

            // Villain mappings
            CreateMap<Villain, VillainDTO>()
                .ForMember(dest => dest.Quirk, opt => opt.MapFrom(src => src.Quirk));

            CreateMap<CreateVillainDTO, Villain>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            CreateMap<UpdateVillainDTO, Villain>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            // Item mappings
            CreateMap<Item, ItemDTO>();
            CreateMap<CreateItemDTO, Item>();
            CreateMap<UpdateItemDTO, Item>();
        }
    }
}