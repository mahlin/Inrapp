using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Inrapporteringsportal.DataAccess.Repositories
{
    internal class Repository<TEntity, TContext> : IRepository<TEntity> where TContext : DbContext where TEntity : class
    {
        private readonly TContext dbContext;

        public Repository(TContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TEntity GetById(object id)
        {
            return dbContext.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbContext.Set<TEntity>().AsEnumerable();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter)
        {
            return dbContext.Set<TEntity>().Where(filter).AsEnumerable();
        }

        public TEntity FindSingle(Expression<Func<TEntity, bool>> filter)
        {
            return dbContext.Set<TEntity>().Where(filter).SingleOrDefault();
        }

        public TEntity Add(TEntity newEntity)
        {
            var newlyCreatedEntity = dbContext.Set<TEntity>().Add(newEntity);
            dbContext.SaveChanges();
            return newlyCreatedEntity;
        }

        public void Update()
        {
            dbContext.SaveChanges(); // Entiteter hämtade via denna DbContext uppdateras här... Ev. kan denna anropas centralt när tjänsteanropet returnerar!
        }

        public void Delete(TEntity entity)
        {
            // TODO: Troligen ska vi här sätta ett datum, status etc, och uppdatera entiteten istället för att göra en Delete i DB!
            dbContext.Set<TEntity>().Remove(entity);
            dbContext.SaveChanges();
        }
    }
}