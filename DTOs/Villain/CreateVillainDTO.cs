using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.DTOs.Villain
{
    public class CreateVillainDTO
    {
        [Required, MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        public required Guid QuirkId { get; set; }

        public IFormFile? Image { get; set; }
    }
}