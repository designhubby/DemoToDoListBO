using DemoToListBE.Data.DomainModel;
using DemoToListBE.Data.Entity;
using DemoToListBE.ExceptionHandle.CustomException;
using System.Linq.Expressions;

namespace DemoToListBE.Data.Repository
{
    public class GenericRepository<TEntity, TDomain> : BaseRepository<TEntity> 
        where TEntity : BaseEntity, new() 
        where TDomain: BaseDomainModel<TEntity>
    {
        public GenericRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<bool> CheckEntityExists(int id)
        {
            bool result = await EntityExists(id);
            return result;
        }
        public async Task<bool> CheckEntityExistsWhere(Expression<Func<TEntity, bool>> predicate)
        {
            bool result = await EntityExistsWhere(predicate);
            return result;
        }
        public TDomain GetDomainModelById(int id)
        {
            TEntity entity = GetEntityById(id);
            if (entity == null)
            {
                throw new ExceptionEntityNotExists("Entity Not Exists");
            }
            return (TDomain)Activator.CreateInstance(typeof(TDomain), entity);
        }
        public async Task<TDomain> GetDomainModelByIdAsync(int id)
        {
            TEntity entity = await GetEntityByIdAsync(id);
            if(entity == null)
            {
                return null;
            }
            return (TDomain)Activator.CreateInstance(typeof(TDomain), entity);
        }
        public async Task<TDomain> GetDomainModelWhereFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            TEntity entity = await GetEntityFirstWhereAync(predicate);
            TDomain result = (TDomain)Activator.CreateInstance(typeof(TDomain), entity);
            return result;

        }
        public async Task<TDomain> AddDomainAsync(TDomain domainModel)
        {
            TEntity _entity = await AddEntityAsync(domainModel._entity);
            return domainModel;//check if the _entity now has the Id
        }
        public async Task UpdateDomainAsync(TDomain domainModel)
        {
            await UpdateEntityAsync(domainModel._entity);
            
        }
    }
}
