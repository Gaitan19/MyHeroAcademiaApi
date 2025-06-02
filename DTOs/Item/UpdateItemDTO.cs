using System.ComponentModel.DataAnnotations;

namespace MyHeroAcademiaApi.DTOs.Item
{
    public class UpdateItemDTO
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [MaxLength(50)]
        public string? Type { get; set; }
    }
}