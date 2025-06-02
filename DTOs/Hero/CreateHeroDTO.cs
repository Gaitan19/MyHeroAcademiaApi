using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.DTOs.Hero
{
    public class CreateHeroDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2-100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Rank is required")]
        [RegularExpression("S|A|B|C", ErrorMessage = "Invalid rank. Must be S, A, B or C")]
        public string Rank { get; set; }

        [Required(ErrorMessage = "Quirk is required")]
        public Guid QuirkId { get; set; }

        [Required(ErrorMessage = "Affiliation is required")]
        [StringLength(50, ErrorMessage = "Affiliation cannot exceed 50 characters")]
        public string Affiliation { get; set; }
    }
}
