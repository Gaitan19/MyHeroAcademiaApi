namespace MyHeroAcademiaApi.Models
{
    public class Villain : BaseEntity
    {
        public string Name { get; set; }
        public string Gang { get; set; }
        public Guid QuirkId { get; set; }
        public Quirk Quirk { get; set; }
    }
}
