using AutoMapper;
using MyHeroAcademiaApi.Data.Repositories;
using MyHeroAcademiaApi.DTOs.Quirk;
using MyHeroAcademiaApi.Models;

namespace MyHeroAcademiaApi.Services
{
    public class QuirkService : IQuirkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<QuirkService> _logger;

        public QuirkService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<QuirkService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<QuirkDTO>> GetAllAsync()
        {
            try
            {
                var quirks = await _unitOfWork.QuirkRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<QuirkDTO>>(quirks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all quirks");
                throw;
            }
        }

        public async Task<QuirkDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var quirk = await _unitOfWork.QuirkRepository.GetByIdAsync(id);

                if (quirk == null || quirk.IsDeleted)
                    throw new KeyNotFoundException($"Quirk with ID {id} not found");

                return _mapper.Map<QuirkDTO>(quirk);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving quirk with ID {id}");
                throw;
            }
        }

        public async Task<QuirkDTO> CreateAsync(CreateQuirkDTO createDto)
        {
            try
            {
                var quirk = _mapper.Map<Quirk>(createDto);
                quirk.Id = Guid.NewGuid();

                await _unitOfWork.QuirkRepository.AddAsync(quirk);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<QuirkDTO>(quirk);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating quirk");
                throw;
            }
        }

        public async Task UpdateAsync(Guid id, QuirkDTO updateDto)
        {
            try
            {
                if (id != updateDto.Id)
                    throw new ArgumentException("ID mismatch");

                var quirk = await _unitOfWork.QuirkRepository.GetByIdAsync(id);
                if (quirk == null || quirk.IsDeleted)
                    throw new KeyNotFoundException($"Quirk with ID {id} not found");

                _mapper.Map(updateDto, quirk);

                _unitOfWork.QuirkRepository.Update(quirk);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating quirk with ID {id}");
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var quirk = await _unitOfWork.QuirkRepository.GetByIdAsync(id);
                if (quirk == null || quirk.IsDeleted)
                    throw new KeyNotFoundException($"Quirk with ID {id} not found");

                // Verificar si el quirk está en uso
                var heroesUsingQuirk = (await _unitOfWork.HeroRepository.GetAllAsync())
                    .Any(h => h.QuirkId == id && !h.IsDeleted);

                var villainsUsingQuirk = (await _unitOfWork.VillainRepository.GetAllAsync())
                    .Any(v => v.QuirkId == id && !v.IsDeleted);

                if (heroesUsingQuirk || villainsUsingQuirk)
                    throw new InvalidOperationException("Cannot delete quirk in use by heroes or villains");

                _unitOfWork.QuirkRepository.Delete(quirk);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting quirk with ID {id}");
                throw;
            }
        }
    }
}
