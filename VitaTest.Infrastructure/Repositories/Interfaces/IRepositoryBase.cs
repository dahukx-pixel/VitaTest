using System.Linq.Expressions;
using VitaTest.Domain.Models;

namespace VitaTest.Infrastructure.Repositories.Interfaces
{
    public interface IRepositoryBase<T> where T : BaseDbEntity
    {
        Task<T?> GetAsync(int id);
        Task<IEnumerable<T>?> GetAsync(Expression<Func<T, bool>> selector);
        Task<List<T>> GetAllAsync();
        Task<T?> AddAsync(T newEntity);
        Task<IEnumerable<T>?> AddRangeAsync(IEnumerable<T> newEntities);
        Task RemoveAsync(int id);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        Task ReplaceAllAsync(IEnumerable<T> entities);
    }
}
