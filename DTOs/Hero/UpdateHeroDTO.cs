using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.DTOs.Hero
{
    public class UpdateHeroDTO : CreateHeroDTO
    {
        [Required(ErrorMessage = "ID is required")]
        public Guid Id { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        public string ImageUrl { get; set; }
    }
}
