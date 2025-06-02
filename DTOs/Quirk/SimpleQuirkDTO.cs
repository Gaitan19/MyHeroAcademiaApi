namespace MyHeroAcademiaApi.DTOs.Quirk
{
    public class SimpleQuirkDTO
    {
        public Guid Id { get; set; }
        public required string Type { get; set; }
    }
}