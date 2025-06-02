using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.DTOs.Quirk
{
    public class CreateQuirkDTO
    {
        [Required, MaxLength(100)]
        public required string Type { get; set; }

        [Required]
        public required string Effects { get; set; }

        public string? Weaknesses { get; set; }
    }
}