using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.DTOs.Item
{
    public class CreateItemDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [RegularExpression("SupportGadget|Uniform", ErrorMessage = "Type must be SupportGadget or Uniform")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public Guid? HeroId { get; set; }
        public Guid? VillainId { get; set; }
    }
}
