using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.DTOs.Quirk
{
    public class QuirkDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [StringLength(50, ErrorMessage = "Type cannot exceed 50 characters")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Effects are required")]
        public string Effects { get; set; }

        [Required(ErrorMessage = "Weaknesses are required")]
        public string Weaknesses { get; set; }
    }
}
