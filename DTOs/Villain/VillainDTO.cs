using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.DTOs.Villain
{
    // DTOs/Villain/VillainDTO.cs
    public class VillainDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2-100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Gang is required")]
        [StringLength(50, ErrorMessage = "Gang cannot exceed 50 characters")]
        public string Gang { get; set; }

        [Required(ErrorMessage = "Quirk is required")]
        public Guid QuirkId { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        public string ImageUrl { get; set; }
    }

    
}
