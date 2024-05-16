﻿using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Interfaces;

namespace ePizzaHub.Services.Implementations
{
    public class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        IRepository<TEntity> _repo;
        public Service(IRepository<TEntity> repository)
        {
            _repo = repository;
        }

        public void Add(TEntity entity)
        {
           _repo.Add(entity);
            _repo.SaveChanges();
        }

        public void Delete(object id)
        {
            _repo?.Delete(id);
            _repo.SaveChanges();
        }

        public TEntity Find(object id)
        {
           return _repo.GetById(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _repo.GetAll();
        }

        public void Update(TEntity entity)
        {
            _repo.Update(entity);
            _repo.SaveChanges();
        }
    }
}
