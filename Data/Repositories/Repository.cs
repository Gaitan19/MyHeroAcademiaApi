using Microsoft.EntityFrameworkCore;
using MyHeroAcademiaApi.Data;
using MyHeroAcademiaApi.Models;

namespace MyHeroAcademiaApi.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
               => await _entities.Where(e => !e.IsDeleted).ToListAsync();

        public async Task<T?> GetByIdAsync(Guid id)
            => await _entities.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        // Nuevo método para incluir propiedades de navegación
        public IQueryable<T> GetQueryable()
            => _entities.AsQueryable().Where(e => !e.IsDeleted);
        public async Task AddAsync(T entity) => await _entities.AddAsync(entity);
        public void Update(T entity) => _entities.Update(entity);
        public void Delete(T entity) => entity.IsDeleted = true;
        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}