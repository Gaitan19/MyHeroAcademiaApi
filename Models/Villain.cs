namespace MyHeroAcademiaApi.Models
{
    public class Villain : BaseEntity
    {
        public required string Name { get; set; }
        public Guid QuirkId { get; set; }
        public Quirk? Quirk { get; set; }
        public string? ImageUrl { get; set; }
    }
}