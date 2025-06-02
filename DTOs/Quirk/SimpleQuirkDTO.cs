namespace MyHeroAcademiaApi.DTOs.Quirk
{
    public class SimpleQuirkDTO
    {
        public Guid Id { get; set; }
        public required string Type { get; set; }

        public required string Effects { get; set; }

        public required string Weaknesses { get; set; }
    }
}