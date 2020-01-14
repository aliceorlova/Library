using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.IRepositories;

namespace DAL.Repositories
{
    internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected AppContext context { get; set; }
        public Repository(AppContext context)
        {
            if (context == null) throw new ArgumentNullException("context error");
            this.context = context;
        }
        public void Create(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }
        public void Delete(TEntity entity)
        {

            context.Set<TEntity>().Remove(entity);
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {

            return await context.Set<TEntity>().ToListAsync();
        }
        public void Update(TEntity entity)
        {

            context.Set<TEntity>().Update(entity);
        }
        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }
    }
}
