using AutoMapper;
using MyHeroAcademiaApi.Data.Repositories;
using MyHeroAcademiaApi.DTOs.Item;
using MyHeroAcademiaApi.Models;

namespace MyHeroAcademiaApi.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ItemService> _logger;

        public ItemService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ItemService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ItemDTO>> GetAllAsync()
        {
            try
            {
                var items = await _unitOfWork.ItemRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<ItemDTO>>(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all items");
                throw;
            }
        }

        public async Task<ItemDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var item = await _unitOfWork.ItemRepository.GetByIdAsync(id);

                if (item == null || item.IsDeleted)
                    throw new KeyNotFoundException($"Item with ID {id} not found");

                return _mapper.Map<ItemDTO>(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving item with ID {id}");
                throw;
            }
        }

        public async Task<ItemDTO> CreateAsync(CreateItemDTO createDto)
        {
            try
            {
                // Validar que el héroe o villano exista si se proporciona
                if (createDto.HeroId.HasValue)
                {
                    var hero = await _unitOfWork.HeroRepository.GetByIdAsync(createDto.HeroId.Value);
                    if (hero == null || hero.IsDeleted)
                        throw new InvalidOperationException($"Hero with ID {createDto.HeroId} not found");
                }

                if (createDto.VillainId.HasValue)
                {
                    var villain = await _unitOfWork.VillainRepository.GetByIdAsync(createDto.VillainId.Value);
                    if (villain == null || villain.IsDeleted)
                        throw new InvalidOperationException($"Villain with ID {createDto.VillainId} not found");
                }

                var item = _mapper.Map<Item>(createDto);
                item.Id = Guid.NewGuid();

                await _unitOfWork.ItemRepository.AddAsync(item);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<ItemDTO>(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating item");
                throw;
            }
        }

        public async Task UpdateAsync(Guid id, ItemDTO updateDto)
        {
            try
            {
                if (id != updateDto.Id)
                    throw new ArgumentException("ID mismatch");

                var item = await _unitOfWork.ItemRepository.GetByIdAsync(id);
                if (item == null || item.IsDeleted)
                    throw new KeyNotFoundException($"Item with ID {id} not found");

                // Validar que el héroe o villano exista si se proporciona
                if (updateDto.HeroId.HasValue)
                {
                    var hero = await _unitOfWork.HeroRepository.GetByIdAsync(updateDto.HeroId.Value);
                    if (hero == null || hero.IsDeleted)
                        throw new InvalidOperationException($"Hero with ID {updateDto.HeroId} not found");
                }

                if (updateDto.VillainId.HasValue)
                {
                    var villain = await _unitOfWork.VillainRepository.GetByIdAsync(updateDto.VillainId.Value);
                    if (villain == null || villain.IsDeleted)
                        throw new InvalidOperationException($"Villain with ID {updateDto.VillainId} not found");
                }

                _mapper.Map(updateDto, item);

                _unitOfWork.ItemRepository.Update(item);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating item with ID {id}");
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var item = await _unitOfWork.ItemRepository.GetByIdAsync(id);
                if (item == null || item.IsDeleted)
                    throw new KeyNotFoundException($"Item with ID {id} not found");

                _unitOfWork.ItemRepository.Delete(item);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting item with ID {id}");
                throw;
            }
        }

        public async Task<IEnumerable<ItemDTO>> GetItemsByOwnerAsync(Guid ownerId, bool isHero)
        {
            try
            {
                IEnumerable<Item> items;

                if (isHero)
                {
                    // Validar que el héroe exista
                    var hero = await _unitOfWork.HeroRepository.GetByIdAsync(ownerId);
                    if (hero == null || hero.IsDeleted)
                        throw new KeyNotFoundException($"Hero with ID {ownerId} not found");

                    items = (await _unitOfWork.ItemRepository.GetAllAsync())
                        .Where(i => i.HeroId == ownerId && !i.IsDeleted);
                }
                else
                {
                    // Validar que el villano exista
                    var villain = await _unitOfWork.VillainRepository.GetByIdAsync(ownerId);
                    if (villain == null || villain.IsDeleted)
                        throw new KeyNotFoundException($"Villain with ID {ownerId} not found");

                    items = (await _unitOfWork.ItemRepository.GetAllAsync())
                        .Where(i => i.VillainId == ownerId && !i.IsDeleted);
                }

                return _mapper.Map<IEnumerable<ItemDTO>>(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving items for owner ID {ownerId} (Type: {(isHero ? "Hero" : "Villain")})");
                throw;
            }
        }
    }
}
