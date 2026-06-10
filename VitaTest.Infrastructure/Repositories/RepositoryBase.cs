using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VitaTest.Domain.Models;
using VitaTest.Infrastructure.Database;
using VitaTest.Infrastructure.Repositories.Interfaces;

namespace VitaTest.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : BaseDbEntity
    {
        protected readonly VitaDatabase Context;

        public RepositoryBase(VitaDatabase context)
        {
            Context = context;
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            try
            {
                return await Context.Set<T>().FirstOrDefaultAsync(x => x.ID == id);
            }
            catch (Exception ex)
            {
                //TODO: logger
                return null;
            }
        }

        public virtual async Task<IEnumerable<T>?> GetAsync(Expression<Func<T, bool>> selector)
        {
            try
            {
                return await Context.Set<T>().Where(selector).ToListAsync();
            }
            catch (Exception ex)
            {
                //TODO: logger
                return null;
            }
        }

        public virtual async Task<IEnumerable<T>?> GetAllAsync()
        {
            try
            {
                return await Context.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                //TODO: logger
                return null;
            }
        }

        public virtual async Task<T?> AddAsync(T newEntity)
        {
            try
            {
                var entity = await Context.Set<T>().AddAsync(newEntity);
                await Context.SaveChangesAsync();
                return entity.Entity;
            }
            catch (Exception ex)
            {
                //TODO: logger
                return null;
            }
        }

        public virtual async Task<IEnumerable<T>?> AddRangeAsync(IEnumerable<T> newEntities)
        {
            try
            {
                await Context.Set<T>().AddRangeAsync(newEntities);
                await Context.SaveChangesAsync();
                return newEntities;
            }
            catch (Exception ex)
            {
                //TODO: logger
                return null;
            }
        }

        public virtual async Task RemoveAsync(int id)
        {
            try
            {
                var entity = await Context.Set<T>().FirstOrDefaultAsync(x => x.ID == id);
                if (entity != null) Context.Set<T>().Remove(entity);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //TODO: logger
            }
        }

        public virtual async Task UpdateAsync(T entity)
        {
            try
            {
                DetachAllEntities();
                Context.Set<T>().Update(entity);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //TODO: logger
            }
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                Context.Set<T>().UpdateRange(entities);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //TODO: logger
            }
        }

        public virtual async Task ReplaceAllAsync(IEnumerable<T> newEntities)
        {
            try
            {
                var set = Context.Set<T>().ToList();
                Context.RemoveRange(set);
                Context.AddRange(newEntities);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //TODO: logger
            }
        }

        private void DetachAllEntities()
        {
            try
            {
                var undetachedEntriesCopy = Context.ChangeTracker.Entries()
                    .Where(e => e.State != EntityState.Detached)
                    .ToList();
                foreach (var entry in undetachedEntriesCopy)
                    entry.State = EntityState.Detached;
            }
            catch (Exception ex)
            {
                //TODO: logger
            }
        }
    }
}
