using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.DTOs.Hero
{
    public class CreateHeroDTO
    {
        [Required, MaxLength(100)]
        public required string Name { get; set; }

        [Required, Range(1, 1000)]
        public required int Rank { get; set; }

        [Required]
        public required Guid QuirkId { get; set; }

        [MaxLength(100)]
        public string? Affiliation { get; set; }

        public IFormFile? Image { get; set; }
    }
}