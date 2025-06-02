namespace MyHeroAcademiaApi.Models
{
    public class Quirk : BaseEntity
    {
        public required string Type { get; set; }
        public required string Effects { get; set; }
        public string? Weaknesses { get; set; }
    }
}