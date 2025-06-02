namespace MyHeroAcademiaApi.Models
{
    public class Quirk : BaseEntity
    {
        public string Type { get; set; }
        public string Effects { get; set; }
        public string Weaknesses { get; set; }
        public ICollection<Hero> Heroes { get; set; } = new List<Hero>();
        public ICollection<Villain> Villains { get; set; } = new List<Villain>();
    }
}
