namespace MyHeroAcademiaApi.Models
{
    public class Item : BaseEntity
    {
        public string Name { get; set; }
        public string Type { get; set; } // SupportGadget/Uniform
        public string Description { get; set; }
        public Guid? HeroId { get; set; }
        public Guid? VillainId { get; set; }
        public Hero Hero { get; set; }
        public Villain Villain { get; set; }
    }
}
