using MyHeroAcademiaApi.DTOs.Hero;

namespace MyHeroAcademiaApi.Services
{
    public interface IHeroService
    {
        Task<IEnumerable<HeroDTO>> GetAllHeroesAsync();
        Task<HeroDTO> GetHeroByIdAsync(Guid id);
        Task<HeroDTO> CreateHeroAsync(CreateHeroDTO createHeroDTO, string imageUrl);
        Task UpdateHeroAsync(Guid id, UpdateHeroDTO updateHeroDTO, string? imageUrl);
        Task DeleteHeroAsync(Guid id);
    }
}
