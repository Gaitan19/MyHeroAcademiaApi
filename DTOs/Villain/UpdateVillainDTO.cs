using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.DTOs.Villain
{
    public class UpdateVillainDTO
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        public Guid? QuirkId { get; set; }

        public IFormFile? Image { get; set; }
    }
}