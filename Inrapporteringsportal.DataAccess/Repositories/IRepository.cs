using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inrapporteringsportal.DataAccess.Repositories
{
    public interface IRepository<TEntity>
    {
        TEntity GetById(object id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter);
        TEntity FindSingle(Expression<Func<TEntity, bool>> filter);
        TEntity Add(TEntity item);
        void Update();
        void Delete(TEntity item); // OBS: Ska eventuellt ej gå att göra delete på de flesta entiteter.
    }
}
