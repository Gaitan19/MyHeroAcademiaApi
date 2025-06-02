using Microsoft.EntityFrameworkCore;
using MyHeroAcademiaApi.Models;

namespace MyHeroAcademiaApi.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _entities.Where(e => !e.IsDeleted).ToListAsync();

        public async Task<T> GetByIdAsync(Guid id) => await _entities.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        public async Task AddAsync(T entity) => await _entities.AddAsync(entity);

        public void Update(T entity) => _entities.Update(entity);

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            Update(entity);
        }

        public async Task<bool> SaveAsync() => await _context.SaveChangesAsync() > 0;
    }
}
