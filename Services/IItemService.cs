using MyHeroAcademiaApi.DTOs.Item;

namespace MyHeroAcademiaApi.Services
{
    public interface IItemService
    {
        Task<IEnumerable<ItemDTO>> GetAllItemsAsync();
        Task<ItemDTO> GetItemByIdAsync(Guid id);
        Task<ItemDTO> CreateItemAsync(CreateItemDTO createItemDTO);
        Task UpdateItemAsync(Guid id, UpdateItemDTO updateItemDTO);
        Task DeleteItemAsync(Guid id);
    }
}
