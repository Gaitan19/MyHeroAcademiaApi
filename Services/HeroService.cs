using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyHeroAcademiaApi.Data.Repositories;
using MyHeroAcademiaApi.DTOs.Hero;
using MyHeroAcademiaApi.Models;
using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.Services
{
    public class HeroService : IHeroService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HeroService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HeroDTO>> GetAllHeroesAsync()
        {
            var heroes = await _unitOfWork.Heroes.GetQueryable()
                .Include(h => h.Quirk)
                .Where(h => !h.IsDeleted)
                .ToListAsync();

            return _mapper.Map<IEnumerable<HeroDTO>>(heroes);
        }

        public async Task<HeroDTO> GetHeroByIdAsync(Guid id)
        {
            var hero = await _unitOfWork.Heroes.GetQueryable()
                .Include(h => h.Quirk)
                .FirstOrDefaultAsync(h => h.Id == id && !h.IsDeleted);

            if (hero == null) throw new KeyNotFoundException("Hero not found");
            return _mapper.Map<HeroDTO>(hero);
        }

        public async Task<HeroDTO> CreateHeroAsync(CreateHeroDTO createHeroDTO, string imageUrl)
        {
            // Validate rank uniqueness
            var existingHero = await _unitOfWork.Heroes.GetQueryable()
                .FirstOrDefaultAsync(h => h.Rank == createHeroDTO.Rank && !h.IsDeleted);

            if (existingHero != null)
                throw new ValidationException("Rank must be unique");

            // Validate quirk exists
            var quirk = await _unitOfWork.Quirks.GetByIdAsync(createHeroDTO.QuirkId);
            if (quirk == null)
                throw new KeyNotFoundException("Quirk not found");

            var hero = _mapper.Map<Hero>(createHeroDTO);
            hero.ImageUrl = imageUrl;

            await _unitOfWork.Heroes.AddAsync(hero);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<HeroDTO>(hero);
        }

        public async Task UpdateHeroAsync(Guid id, UpdateHeroDTO updateHeroDTO, string? imageUrl)
        {
            var hero = await _unitOfWork.Heroes.GetByIdAsync(id);
            if (hero == null) throw new KeyNotFoundException("Hero not found");

            // Validate rank if changed
            if (updateHeroDTO.Rank.HasValue && hero.Rank != updateHeroDTO.Rank.Value)
            {
                var existingHero = await _unitOfWork.Heroes.GetQueryable()
                    .FirstOrDefaultAsync(h => h.Rank == updateHeroDTO.Rank.Value && !h.IsDeleted);

                if (existingHero != null)
                    throw new ValidationException("Rank must be unique");

                hero.Rank = updateHeroDTO.Rank.Value;
            }

            // Update other properties
            if (!string.IsNullOrEmpty(updateHeroDTO.Name))
                hero.Name = updateHeroDTO.Name;

            if (updateHeroDTO.QuirkId.HasValue)
            {
                var quirk = await _unitOfWork.Quirks.GetByIdAsync(updateHeroDTO.QuirkId.Value);
                if (quirk == null) throw new KeyNotFoundException("Quirk not found");
                hero.QuirkId = updateHeroDTO.QuirkId.Value;
            }

            if (!string.IsNullOrEmpty(updateHeroDTO.Affiliation))
                hero.Affiliation = updateHeroDTO.Affiliation;

            if (!string.IsNullOrEmpty(imageUrl))
                hero.ImageUrl = imageUrl;

            _unitOfWork.Heroes.Update(hero);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteHeroAsync(Guid id)
        {
            var hero = await _unitOfWork.Heroes.GetByIdAsync(id);
            if (hero == null) throw new KeyNotFoundException("Hero not found");

            _unitOfWork.Heroes.Delete(hero);
            await _unitOfWork.CompleteAsync();
        }
    }
}