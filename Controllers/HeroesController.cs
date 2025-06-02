using Microsoft.AspNetCore.Mvc;
using MyHeroAcademiaApi.DTOs.Hero;
using MyHeroAcademiaApi.Services;
using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroService _heroService;
        private readonly IWebHostEnvironment _env;

        public HeroesController(IHeroService heroService, IWebHostEnvironment env)
        {
            _heroService = heroService;
            _env = env;
        }

        /// <summary>
        /// Get all heroes
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllHeroes()
        {
            var heroes = await _heroService.GetAllHeroesAsync();
            return Ok(heroes);
        }

        /// <summary>
        /// Get a hero by ID
        /// </summary>
        /// <param name="id">Hero ID</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHeroById(Guid id)
        {
            try
            {
                var hero = await _heroService.GetHeroByIdAsync(id);
                return Ok(hero);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Create a new hero
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateHero([FromForm] CreateHeroDTO createHeroDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (createHeroDTO.Image == null || createHeroDTO.Image.Length == 0)
                return BadRequest("Image is required");

            var imageUrl = await SaveImage(createHeroDTO.Image);

            try
            {
                var hero = await _heroService.CreateHeroAsync(createHeroDTO, imageUrl);
                return CreatedAtAction(nameof(GetHeroById), new { id = hero.Id }, hero);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update a hero
        /// </summary>
        /// <param name="id">Hero ID</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHero(Guid id, [FromForm] UpdateHeroDTO updateHeroDTO)
        {
            string? imageUrl = null;
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (updateHeroDTO.Image != null && updateHeroDTO.Image.Length > 0)
            {
                imageUrl = await SaveImage(updateHeroDTO.Image);
            }

            try
            {
                await _heroService.UpdateHeroAsync(id, updateHeroDTO, imageUrl);
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
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a hero
        /// </summary>
        /// <param name="id">Hero ID</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHero(Guid id)
        {
            try
            {
                await _heroService.DeleteHeroAsync(id);
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