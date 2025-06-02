using System.ComponentModel.DataAnnotations;
using MyHeroAcademiaApi.DTOs.Quirk; // Agregar esta referencia

namespace MyHeroAcademiaApi.DTOs.Hero
{
    public class HeroDTO
    {
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public required string Name { get; set; }

        [Required, Range(1, 1000)]
        public int Rank { get; set; }

        [Required]
        public required SimpleQuirkDTO Quirk { get; set; } // Usar SimpleQuirkDTO

        [MaxLength(100)]
        public string? Affiliation { get; set; }

        [Url]
        public string? ImageUrl { get; set; }
    }
}

// Quitamos la clase anidada QuirkInfoDTO