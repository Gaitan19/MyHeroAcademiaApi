using MyHeroAcademiaApi.DTOs.Quirk;

namespace MyHeroAcademiaApi.Services
{
    public interface IQuirkService
    {
        Task<IEnumerable<QuirkDTO>> GetAllQuirksAsync();
        Task<QuirkDTO> GetQuirkByIdAsync(Guid id);
        Task<QuirkDTO> CreateQuirkAsync(CreateQuirkDTO createQuirkDTO);
        Task UpdateQuirkAsync(Guid id, UpdateQuirkDTO updateQuirkDTO);
        Task DeleteQuirkAsync(Guid id);
    }
}
