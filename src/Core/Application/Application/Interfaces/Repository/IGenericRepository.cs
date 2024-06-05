using Core.Domain.Entities;
using System.Linq.Expressions;

namespace Core.Application.Interfaces.Repository
{
    public interface IGenericRepository<T> where T :BaseEntity
    {
        public Task<T> GetSingleAsync(Expression<Func<T, bool>> filter);
        public Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        public Task<T> UpdateAsync(T entity);

    }
}
