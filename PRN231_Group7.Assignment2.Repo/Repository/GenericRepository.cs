using Microsoft.EntityFrameworkCore;
using PRN231_Group7.Assignment2.Repo.DataAccess;
using PRN231_Group7.Assignment2.Repo.Repository.Interface;
using System.Linq.Expressions;

namespace PRN231_Group7.Assignment2.Repo.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal readonly BookDbContext context;
        internal DbSet<TEntity> dbSet;


        public GenericRepository(BookDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>> orderBy = null,
            bool? orderByAsc = true,
            string includeProperties = null,
            int? pageIndex = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null) query = query.Where(filter);
            if (includeProperties != null)
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

            if (orderBy != null)
                query = orderByAsc == false ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);

            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10;
                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return query.ToList();
        }

        public virtual TEntity GetById(Guid id) => dbSet.Find(id);

        public virtual void Insert(TEntity entity) => dbSet.Add(entity);

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            dbSet.Remove(entityToDelete);
        }

        public virtual void Delete(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

    }
}
