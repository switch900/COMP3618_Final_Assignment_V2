using IMDbDotNetDomain;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace IMDbDotNetInfrastructure
{
    public class EFGenericRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
    {
        private DbContext Context { get; set; }
        public EFGenericRepository(DbContext inContext)
        {
            Context = inContext;
        }
        public void Create(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public TEntity Read(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).FirstOrDefault();
        }

        public IQueryable<TEntity> Reads(string predicate, int startindex, int pagesize)
        {
            return Context.Set<TEntity>().SqlQuery("Select * from [IMDb].[movie].[titlebasics] where tconst like '%" + predicate + "%' order by tconst offset " + startindex + " rows fetch next " + pagesize + " rows only").AsQueryable();
        }

        public IQueryable<TEntity> Reads(string predicate)
        {
            return Context.Set<TEntity>().SqlQuery("Select * from [IMDb].[movie].[titlebasics] where tconst like '%" + predicate + "%'").AsQueryable();
        }

        public void Update(TEntity entity)
        {
            Context.Entry<TEntity>(entity).State = EntityState.Modified;
        }

        public void Update(TEntity entity, Expression<Func<TEntity, object>>[] updateProperties)
        {
            Context.Configuration.ValidateOnSaveEnabled = false;

            Context.Entry<TEntity>(entity).State = EntityState.Unchanged;

            if (updateProperties != null)
            {
                foreach (var property in updateProperties)
                {
                    Context.Entry<TEntity>(entity).Property(property).IsModified = true;
                }
            }
        }

        public void Delete(TEntity entity)
        {
            Context.Entry<TEntity>(entity).State = EntityState.Deleted;
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
            if (Context.Configuration.ValidateOnSaveEnabled == false)
            {
                Context.Configuration.ValidateOnSaveEnabled = true;
            }
        }
    }
}
