using MyHeroAcademiaApi.Models;

namespace MyHeroAcademiaApi.Data.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity); // Soft delete
        Task<bool> SaveAsync();
    }
}
