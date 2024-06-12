using Core.Application.Interfaces.Repository;
using Core.Domain.Entities;
using Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistance.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly TicketDbContext _context;
        private DbSet<T> table => _context.Set<T>();


        public GenericRepository(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> filter=null)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();
            if (filter != null)
                query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter=null)
        {
            IQueryable<T> query = table;
            query.AsNoTracking();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<T> UpdateAsync(T entity)
        {
            table.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }


}
