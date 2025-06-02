using Microsoft.AspNetCore.Mvc;
using MyHeroAcademiaApi.DTOs.Quirk;
using MyHeroAcademiaApi.Services;
using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuirksController : ControllerBase
    {
        private readonly IQuirkService _quirkService;

        public QuirksController(IQuirkService quirkService)
        {
            _quirkService = quirkService;
        }

        /// <summary>
        /// Get all quirks
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllQuirks()
        {
            var quirks = await _quirkService.GetAllQuirksAsync();
            return Ok(quirks);
        }

        /// <summary>
        /// Get a quirk by ID
        /// </summary>
        /// <param name="id">Quirk ID</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuirkById(Guid id)
        {
            try
            {
                var quirk = await _quirkService.GetQuirkByIdAsync(id);
                return Ok(quirk);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Create a new quirk
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateQuirk([FromBody] CreateQuirkDTO createQuirkDTO)
        {
            try
            {
                var quirk = await _quirkService.CreateQuirkAsync(createQuirkDTO);
                return CreatedAtAction(nameof(GetQuirkById), new { id = quirk.Id }, quirk);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update a quirk
        /// </summary>
        /// <param name="id">Quirk ID</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuirk(Guid id, [FromBody] UpdateQuirkDTO updateQuirkDTO)
        {
            try
            {
                await _quirkService.UpdateQuirkAsync(id, updateQuirkDTO);
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
        /// Delete a quirk
        /// </summary>
        /// <param name="id">Quirk ID</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuirk(Guid id)
        {
            try
            {
                await _quirkService.DeleteQuirkAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}