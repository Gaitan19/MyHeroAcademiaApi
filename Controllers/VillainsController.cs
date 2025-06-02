using Microsoft.AspNetCore.Mvc;
using MyHeroAcademiaApi.DTOs.Villain;
using MyHeroAcademiaApi.Services;
using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VillainsController : ControllerBase
    {
        private readonly IVillainService _villainService;
        private readonly IWebHostEnvironment _env;

        public VillainsController(IVillainService villainService, IWebHostEnvironment env)
        {
            _villainService = villainService;
            _env = env;
        }

        /// <summary>
        /// Get all villains
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllVillains()
        {
            var villains = await _villainService.GetAllVillainsAsync();
            return Ok(villains);
        }

        /// <summary>
        /// Get a villain by ID
        /// </summary>
        /// <param name="id">Villain ID</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVillainById(Guid id)
        {
            try
            {
                var villain = await _villainService.GetVillainByIdAsync(id);
                return Ok(villain);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Create a new villain
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateVillain([FromForm] CreateVillainDTO createVillainDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (createVillainDTO.Image == null || createVillainDTO.Image.Length == 0)
                return BadRequest("Image is required");

            var imageUrl = await SaveImage(createVillainDTO.Image);

            try
            {
                var villain = await _villainService.CreateVillainAsync(createVillainDTO, imageUrl);
                return CreatedAtAction(nameof(GetVillainById), new { id = villain.Id }, villain);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Update a villain
        /// </summary>
        /// <param name="id">Villain ID</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVillain(Guid id, [FromForm] UpdateVillainDTO updateVillainDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? imageUrl = null;
            if (updateVillainDTO.Image != null && updateVillainDTO.Image.Length > 0)
            {
                imageUrl = await SaveImage(updateVillainDTO.Image);
            }

            try
            {
                await _villainService.UpdateVillainAsync(id, updateVillainDTO, imageUrl);
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
        /// Delete a villain
        /// </summary>
        /// <param name="id">Villain ID</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVillain(Guid id)
        {
            try
            {
                await _villainService.DeleteVillainAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/uploads/{uniqueFileName}";
        }
    }
}