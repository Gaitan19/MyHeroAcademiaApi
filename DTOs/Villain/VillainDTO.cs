using System.ComponentModel.DataAnnotations;
using MyHeroAcademiaApi.DTOs.Quirk; // Agregar esta referencia

namespace MyHeroAcademiaApi.DTOs.Villain
{
    public class VillainDTO
    {
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        public required SimpleQuirkDTO Quirk { get; set; } // Usar SimpleQuirkDTO

        [Url]
        public string? ImageUrl { get; set; }
    }
}

// Quitamos la clase anidada QuirkInfoDTO