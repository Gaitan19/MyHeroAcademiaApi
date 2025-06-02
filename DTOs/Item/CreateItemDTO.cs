using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.DTOs.Item
{
    public class CreateItemDTO
    {
        [Required, MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        [MaxLength(50)]
        public string? Type { get; set; }
    }
}