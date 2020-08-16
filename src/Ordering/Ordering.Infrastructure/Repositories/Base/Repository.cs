using Ordering.Core.Entities.Base;
using Ordering.Core.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Infrastructure.Data;
// ReSharper disable MethodOverloadWithOptionalParameter

namespace Ordering.Infrastructure.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly OrderContext _dbContext;

        public Repository(OrderContext dbContext) => _dbContext = dbContext;

        public async Task<IReadOnlyList<T>> GetAllAsync() => await _dbContext.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate) => await _dbContext.Set<T>().Where(predicate).ToListAsync();

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (disableTracking) query = query.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);
             
            if (predicate is { }) query = query.Where(predicate);
            if (orderBy is { }) query = orderBy(query);

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (disableTracking) query = query.AsNoTracking();
            if (includes is { }) query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (predicate is { }) query = query.Where(predicate);
            if (orderBy is { }) query = orderBy(query);

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id) => await _dbContext.Set<T>().FindAsync(id);

        public async Task<T> AddAsync(T entity)
        {
            var createdEntity = await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return createdEntity.Entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
