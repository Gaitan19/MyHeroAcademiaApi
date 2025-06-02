using MyHeroAcademiaApi.Models;

namespace MyHeroAcademiaApi.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed = false;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Heroes = new Repository<Hero>(context);
            Quirks = new Repository<Quirk>(context);
            Villains = new Repository<Villain>(context);
            Items = new Repository<Item>(context);
        }

        public IRepository<Hero> Heroes { get; }
        public IRepository<Quirk> Quirks { get; }
        public IRepository<Villain> Villains { get; }
        public IRepository<Item> Items { get; }

        public async Task CompleteAsync() => await _context.SaveChangesAsync();

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}