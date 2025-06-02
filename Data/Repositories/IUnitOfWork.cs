using MyHeroAcademiaApi.Models;

namespace MyHeroAcademiaApi.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Hero> HeroRepository { get; }
        IRepository<Quirk> QuirkRepository { get; }
        IRepository<Villain> VillainRepository { get; }
        IRepository<Item> ItemRepository { get; }
        Task<bool> SaveAsync();
    }
}
