using AutoMapper;
using MyHeroAcademiaApi.Data.Repositories;
using MyHeroAcademiaApi.DTOs.Item;
using MyHeroAcademiaApi.Models;
using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemDTO>> GetAllItemsAsync()
        {
            var items = await _unitOfWork.Items.GetAllAsync();
            return _mapper.Map<IEnumerable<ItemDTO>>(items);
        }

        public async Task<ItemDTO> GetItemByIdAsync(Guid id)
        {
            var item = await _unitOfWork.Items.GetByIdAsync(id);
            if (item == null) throw new KeyNotFoundException("Item not found");
            return _mapper.Map<ItemDTO>(item);
        }

        public async Task<ItemDTO> CreateItemAsync(CreateItemDTO createItemDTO)
        {
            var item = _mapper.Map<Item>(createItemDTO);
            await _unitOfWork.Items.AddAsync(item);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ItemDTO>(item);
        }

        public async Task UpdateItemAsync(Guid id, UpdateItemDTO updateItemDTO)
        {
            var item = await _unitOfWork.Items.GetByIdAsync(id);
            if (item == null) throw new KeyNotFoundException("Item not found");

            // Update properties
            if (!string.IsNullOrEmpty(updateItemDTO.Name))
                item.Name = updateItemDTO.Name;

            if (!string.IsNullOrEmpty(updateItemDTO.Description))
                item.Description = updateItemDTO.Description;

            if (!string.IsNullOrEmpty(updateItemDTO.Type))
                item.Type = updateItemDTO.Type;

            _unitOfWork.Items.Update(item);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var item = await _unitOfWork.Items.GetByIdAsync(id);
            if (item == null) throw new KeyNotFoundException("Item not found");

            _unitOfWork.Items.Delete(item);
            await _unitOfWork.CompleteAsync();
        }
    }
}