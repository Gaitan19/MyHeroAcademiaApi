namespace MyHeroAcademiaApi.Models
{
    public class Item : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? Type { get; set; } // gadget/uniforme

    }
}