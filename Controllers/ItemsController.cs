using Microsoft.AspNetCore.Mvc;
using MyHeroAcademiaApi.DTOs.Item;
using MyHeroAcademiaApi.Services;
using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Get all items
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

        /// <summary>
        /// Get an item by ID
        /// </summary>
        /// <param name="id">Item ID</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(Guid id)
        {
            try
            {
                var item = await _itemService.GetItemByIdAsync(id);
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Create a new item
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemDTO createItemDTO)
        {
            try
            {
                var item = await _itemService.CreateItemAsync(createItemDTO);
                return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update an item
        /// </summary>
        /// <param name="id">Item ID</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, [FromBody] UpdateItemDTO updateItemDTO)
        {
            try
            {
                await _itemService.UpdateItemAsync(id, updateItemDTO);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete an item
        /// </summary>
        /// <param name="id">Item ID</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            try
            {
                await _itemService.DeleteItemAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}