using MyHeroAcademiaApi.Models;

namespace MyHeroAcademiaApi.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            HeroRepository = new Repository<Hero>(_context);
            QuirkRepository = new Repository<Quirk>(_context);
            VillainRepository = new Repository<Villain>(_context);
            ItemRepository = new Repository<Item>(_context);
        }

        public IRepository<Hero> HeroRepository { get; }
        public IRepository<Quirk> QuirkRepository { get; }
        public IRepository<Villain> VillainRepository { get; }
        public IRepository<Item> ItemRepository { get; }

        public async Task<bool> SaveAsync() => await _context.SaveChangesAsync() > 0;

        public void Dispose() => _context.Dispose();
    }
}
