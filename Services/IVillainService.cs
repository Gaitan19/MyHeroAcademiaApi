using MyHeroAcademiaApi.DTOs.Villain;

namespace MyHeroAcademiaApi.Services
{
    public interface IVillainService
    {
        Task<IEnumerable<VillainDTO>> GetAllVillainsAsync();
        Task<VillainDTO> GetVillainByIdAsync(Guid id);
        Task<VillainDTO> CreateVillainAsync(CreateVillainDTO createVillainDTO, string imageUrl);
        Task UpdateVillainAsync(Guid id, UpdateVillainDTO updateVillainDTO, string? imageUrl);
        Task DeleteVillainAsync(Guid id);
    }
}
