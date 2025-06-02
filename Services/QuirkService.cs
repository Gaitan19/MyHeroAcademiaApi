using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyHeroAcademiaApi.Data.Repositories;
using MyHeroAcademiaApi.DTOs.Quirk;
using MyHeroAcademiaApi.Models;
using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.Services
{
    public class QuirkService : IQuirkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuirkService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<QuirkDTO>> GetAllQuirksAsync()
        {
            var quirks = await _unitOfWork.Quirks.GetAllAsync();
            return _mapper.Map<IEnumerable<QuirkDTO>>(quirks);
        }

        public async Task<QuirkDTO> GetQuirkByIdAsync(Guid id)
        {
            var quirk = await _unitOfWork.Quirks.GetByIdAsync(id);
            if (quirk == null) throw new KeyNotFoundException("Quirk not found");
            return _mapper.Map<QuirkDTO>(quirk);
        }

        public async Task<QuirkDTO> CreateQuirkAsync(CreateQuirkDTO createQuirkDTO)
        {
            var quirk = _mapper.Map<Quirk>(createQuirkDTO);
            await _unitOfWork.Quirks.AddAsync(quirk);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<QuirkDTO>(quirk);
        }

        public async Task UpdateQuirkAsync(Guid id, UpdateQuirkDTO updateQuirkDTO)
        {
            var quirk = await _unitOfWork.Quirks.GetByIdAsync(id);
            if (quirk == null) throw new KeyNotFoundException("Quirk not found");

            // Update properties
            if (!string.IsNullOrEmpty(updateQuirkDTO.Type))
                quirk.Type = updateQuirkDTO.Type;

            if (!string.IsNullOrEmpty(updateQuirkDTO.Effects))
                quirk.Effects = updateQuirkDTO.Effects;

            if (updateQuirkDTO.Weaknesses != null)
                quirk.Weaknesses = updateQuirkDTO.Weaknesses;

            _unitOfWork.Quirks.Update(quirk);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteQuirkAsync(Guid id)
        {
            var quirk = await _unitOfWork.Quirks.GetByIdAsync(id);
            if (quirk == null) throw new KeyNotFoundException("Quirk not found");

            // Check if any hero or villain is using this quirk
            var heroUsingQuirk = await _unitOfWork.Heroes.GetQueryable()
                .AnyAsync(h => h.QuirkId == id && !h.IsDeleted);

            var villainUsingQuirk = await _unitOfWork.Villains.GetQueryable()
                .AnyAsync(v => v.QuirkId == id && !v.IsDeleted);

            if (heroUsingQuirk || villainUsingQuirk)
                throw new InvalidOperationException("Cannot delete quirk that is currently assigned to a hero or villain");

            _unitOfWork.Quirks.Delete(quirk);
            await _unitOfWork.CompleteAsync();
        }
    }
}