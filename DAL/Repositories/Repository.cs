using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.IRepositories;

namespace DAL.Repositories
{
    internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected AppContext context { get; set; }
        public Repository(AppContext context)
        {
            if (context == null) throw new ArgumentNullException("context error ");
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

        public async Task<IEnumerable<TEntity>> GetAll()
        {

            return await context.Set<TEntity>().ToListAsync();
        }

        public void Update(TEntity entity)
        {

            context.Set<TEntity>().Update(entity);
        }

        public async Task<TEntity> GetById(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }
    }
}
