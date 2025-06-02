using AutoMapper;
using MyHeroAcademiaApi.Data.Repositories;
using MyHeroAcademiaApi.DTOs.Hero;
using MyHeroAcademiaApi.Models;

namespace MyHeroAcademiaApi.Services
{
    public class HeroService : IHeroService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;
        private readonly ILogger<HeroService> _logger;

        public HeroService(IUnitOfWork unitOfWork, IMapper mapper, ImageService imageService, ILogger<HeroService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
            _logger = logger;
        }

        public async Task<IEnumerable<HeroDTO>> GetAllAsync()
        {
            try
            {
                var heroes = await _unitOfWork.HeroRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<HeroDTO>>(heroes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all heroes");
                throw;
            }
        }

        public async Task<HeroDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var hero = await _unitOfWork.HeroRepository.GetByIdAsync(id);

                if (hero == null || hero.IsDeleted)
                    throw new KeyNotFoundException($"Hero with ID {id} not found");

                return _mapper.Map<HeroDTO>(hero);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving hero with ID {id}");
                throw;
            }
        }

        public async Task<HeroDTO> CreateAsync(CreateHeroDTO createDto, IFormFile imageFile = null)
        {
            try
            {
                // Validar existencia de quirk
                var quirk = await _unitOfWork.QuirkRepository.GetByIdAsync(createDto.QuirkId);
                if (quirk == null || quirk.IsDeleted)
                    throw new InvalidOperationException($"Quirk with ID {createDto.QuirkId} does not exist");

                var hero = _mapper.Map<Hero>(createDto);
                hero.Id = Guid.NewGuid();

                // Guardar imagen si se proporciona
                if (imageFile != null)
                {
                    hero.ImageUrl = await _imageService.SaveImageAsync(imageFile, "Hero");
                }

                await _unitOfWork.HeroRepository.AddAsync(hero);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<HeroDTO>(hero);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating hero");
                throw;
            }
        }

        public async Task UpdateAsync(Guid id, UpdateHeroDTO updateDto, IFormFile imageFile = null)
        {
            try
            {
                if (id != updateDto.Id)
                    throw new ArgumentException("ID mismatch");

                var hero = await _unitOfWork.HeroRepository.GetByIdAsync(id);
                if (hero == null || hero.IsDeleted)
                    throw new KeyNotFoundException($"Hero with ID {id} not found");

                // Validar existencia de quirk
                var quirk = await _unitOfWork.QuirkRepository.GetByIdAsync(updateDto.QuirkId);
                if (quirk == null || quirk.IsDeleted)
                    throw new InvalidOperationException($"Quirk with ID {updateDto.QuirkId} does not exist");

                _mapper.Map(updateDto, hero);

                // Actualizar imagen si se proporciona
                if (imageFile != null)
                {
                    hero.ImageUrl = await _imageService.SaveImageAsync(imageFile, "Hero");
                }

                _unitOfWork.HeroRepository.Update(hero);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating hero with ID {id}");
                throw;
            }
        }

        public async Task UpdateImageAsync(Guid id, IFormFile imageFile)
        {
            try
            {
                var hero = await _unitOfWork.HeroRepository.GetByIdAsync(id);
                if (hero == null || hero.IsDeleted)
                    throw new KeyNotFoundException($"Hero with ID {id} not found");

                hero.ImageUrl = await _imageService.SaveImageAsync(imageFile, "Hero");

                _unitOfWork.HeroRepository.Update(hero);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating image for hero with ID {id}");
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var hero = await _unitOfWork.HeroRepository.GetByIdAsync(id);
                if (hero == null || hero.IsDeleted)
                    throw new KeyNotFoundException($"Hero with ID {id} not found");

                _unitOfWork.HeroRepository.Delete(hero);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting hero with ID {id}");
                throw;
            }
        }
    }
}
