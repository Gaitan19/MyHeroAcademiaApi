using AutoMapper;
using MyHeroAcademiaApi.Data.Repositories;
using MyHeroAcademiaApi.DTOs.Villain;
using MyHeroAcademiaApi.Models;

namespace MyHeroAcademiaApi.Services
{
    public class VillainService : IVillainService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;
        private readonly ILogger<VillainService> _logger;

        public VillainService(IUnitOfWork unitOfWork, IMapper mapper, ImageService imageService, ILogger<VillainService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
            _logger = logger;
        }

        public async Task<IEnumerable<VillainDTO>> GetAllAsync()
        {
            try
            {
                var villains = await _unitOfWork.VillainRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<VillainDTO>>(villains);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all villains");
                throw;
            }
        }

        public async Task<VillainDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var villain = await _unitOfWork.VillainRepository.GetByIdAsync(id);

                if (villain == null || villain.IsDeleted)
                    throw new KeyNotFoundException($"Villain with ID {id} not found");

                return _mapper.Map<VillainDTO>(villain);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving villain with ID {id}");
                throw;
            }
        }

        public async Task<VillainDTO> CreateAsync(CreateVillainDTO createDto, IFormFile imageFile = null)
        {
            try
            {
                // Validar existencia de quirk
                var quirk = await _unitOfWork.QuirkRepository.GetByIdAsync(createDto.QuirkId);
                if (quirk == null || quirk.IsDeleted)
                    throw new InvalidOperationException($"Quirk with ID {createDto.QuirkId} does not exist");

                var villain = _mapper.Map<Villain>(createDto);
                villain.Id = Guid.NewGuid();

                // Guardar imagen si se proporciona
                if (imageFile != null)
                {
                    villain.ImageUrl = await _imageService.SaveImageAsync(imageFile, "Villain");
                }

                await _unitOfWork.VillainRepository.AddAsync(villain);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<VillainDTO>(villain);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating villain");
                throw;
            }
        }

        public async Task UpdateAsync(Guid id, VillainDTO updateDto, IFormFile imageFile = null)
        {
            try
            {
                if (id != updateDto.Id)
                    throw new ArgumentException("ID mismatch");

                var villain = await _unitOfWork.VillainRepository.GetByIdAsync(id);
                if (villain == null || villain.IsDeleted)
                    throw new KeyNotFoundException($"Villain with ID {id} not found");

                // Validar existencia de quirk
                var quirk = await _unitOfWork.QuirkRepository.GetByIdAsync(updateDto.QuirkId);
                if (quirk == null || quirk.IsDeleted)
                    throw new InvalidOperationException($"Quirk with ID {updateDto.QuirkId} does not exist");

                _mapper.Map(updateDto, villain);

                // Actualizar imagen si se proporciona
                if (imageFile != null)
                {
                    villain.ImageUrl = await _imageService.SaveImageAsync(imageFile, "Villain");
                }

                _unitOfWork.VillainRepository.Update(villain);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating villain with ID {id}");
                throw;
            }
        }

        public async Task UpdateImageAsync(Guid id, IFormFile imageFile)
        {
            try
            {
                var villain = await _unitOfWork.VillainRepository.GetByIdAsync(id);
                if (villain == null || villain.IsDeleted)
                    throw new KeyNotFoundException($"Villain with ID {id} not found");

                villain.ImageUrl = await _imageService.SaveImageAsync(imageFile, "Villain");

                _unitOfWork.VillainRepository.Update(villain);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating image for villain with ID {id}");
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var villain = await _unitOfWork.VillainRepository.GetByIdAsync(id);
                if (villain == null || villain.IsDeleted)
                    throw new KeyNotFoundException($"Villain with ID {id} not found");

                _unitOfWork.VillainRepository.Delete(villain);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting villain with ID {id}");
                throw;
            }
        }
    }
}
