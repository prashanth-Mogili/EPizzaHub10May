using ePizzaHub.Core;
using ePizzaHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Repositories.Implementations
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected AppDbContext _context;      

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity)
        {
             _context.Set<TEntity>().Add(entity);
        }

        public void Delete(object id)
        {
           TEntity entity = _context.Set<TEntity>().Find(id);
            if(entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
           return _context.Set<TEntity>().ToList();
        }

        public TEntity GetById(object id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }
    }
}
