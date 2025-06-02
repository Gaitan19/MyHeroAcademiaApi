using MyHeroAcademiaApi.Models;

namespace MyHeroAcademiaApi.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Hero> Heroes { get; }
        IRepository<Quirk> Quirks { get; }
        IRepository<Villain> Villains { get; }
        IRepository<Item> Items { get; }
        Task CompleteAsync();
    }
}