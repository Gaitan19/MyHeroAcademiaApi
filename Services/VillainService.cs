using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyHeroAcademiaApi.Data.Repositories;
using MyHeroAcademiaApi.DTOs.Villain;
using MyHeroAcademiaApi.Models;
using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.Services
{
    public class VillainService : IVillainService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VillainService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VillainDTO>> GetAllVillainsAsync()
        {
            var villains = await _unitOfWork.Villains.GetQueryable()
                .Include(v => v.Quirk)
                .Where(v => !v.IsDeleted)
                .ToListAsync();

            return _mapper.Map<IEnumerable<VillainDTO>>(villains);
        }

        public async Task<VillainDTO> GetVillainByIdAsync(Guid id)
        {
            var villain = await _unitOfWork.Villains.GetQueryable()
                .Include(v => v.Quirk)
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);

            if (villain == null) throw new KeyNotFoundException("Villain not found");
            return _mapper.Map<VillainDTO>(villain);
        }

        public async Task<VillainDTO> CreateVillainAsync(CreateVillainDTO createVillainDTO, string imageUrl)
        {
            // Validate quirk exists
            var quirk = await _unitOfWork.Quirks.GetByIdAsync(createVillainDTO.QuirkId);
            if (quirk == null)
                throw new KeyNotFoundException("Quirk not found");

            var villain = _mapper.Map<Villain>(createVillainDTO);
            villain.ImageUrl = imageUrl;

            await _unitOfWork.Villains.AddAsync(villain);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<VillainDTO>(villain);
        }

        public async Task UpdateVillainAsync(Guid id, UpdateVillainDTO updateVillainDTO, string? imageUrl)
        {
            var villain = await _unitOfWork.Villains.GetByIdAsync(id);
            if (villain == null) throw new KeyNotFoundException("Villain not found");

            // Update properties
            if (!string.IsNullOrEmpty(updateVillainDTO.Name))
                villain.Name = updateVillainDTO.Name;

            if (updateVillainDTO.QuirkId.HasValue)
            {
                var quirk = await _unitOfWork.Quirks.GetByIdAsync(updateVillainDTO.QuirkId.Value);
                if (quirk == null) throw new KeyNotFoundException("Quirk not found");
                villain.QuirkId = updateVillainDTO.QuirkId.Value;
            }

            if (!string.IsNullOrEmpty(imageUrl))
                villain.ImageUrl = imageUrl;

            _unitOfWork.Villains.Update(villain);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteVillainAsync(Guid id)
        {
            var villain = await _unitOfWork.Villains.GetByIdAsync(id);
            if (villain == null) throw new KeyNotFoundException("Villain not found");

            _unitOfWork.Villains.Delete(villain);
            await _unitOfWork.CompleteAsync();
        }
    }
}