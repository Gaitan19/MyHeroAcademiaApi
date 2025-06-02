using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.DTOs.Quirk
{
    public class UpdateQuirkDTO
    {
        [MaxLength(100)]
        public string? Type { get; set; }

        public string? Effects { get; set; }

        public string? Weaknesses { get; set; }
    }
}