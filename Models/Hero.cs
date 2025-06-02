namespace MyHeroAcademiaApi.Models
{
    public class Hero : BaseEntity
    {
        public required string Name { get; set; }
        public int Rank { get; set; } // Unique 1-1000
        public Guid QuirkId { get; set; }
        public Quirk? Quirk { get; set; }
        public string? Affiliation { get; set; }
        public string? ImageUrl { get; set; }
    }
}