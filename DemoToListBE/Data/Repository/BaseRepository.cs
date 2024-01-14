using DemoToListBE.Data.Entity;
using DemoToListBE.Dto.Responses;
using DemoToListBE.ExceptionHandle.CustomException;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace DemoToListBE.Data.Repository
{
    public class BaseRepository<TEntity> where TEntity : BaseEntity
    {
        public ApplicationDbContext _context { get; set; }
        public DbSet<TEntity> _dbSet;
        public BaseRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }
        public async Task<bool> EntityExists(int id)
        {
            bool exists = await _dbSet.AnyAsync(indiv => indiv.Id == id);
            return exists;
        }
        public async Task<bool> EntityExistsWhere(Expression<Func<TEntity, bool>> predicate)
        {
            bool exists = await _dbSet.AnyAsync(predicate);
            return exists;
        }
        public TEntity GetEntityById(int id)
        {
            var entity = _dbSet.Find(id);
            return entity;
        }
        public async Task<TEntity> GetEntityByIdAsync(int id)
        {
            TEntity entity = await _dbSet.FindAsync(id);
            return entity;
        }
        public async Task<TEntity> GetEntityFirstWhereAync(Expression<Func<TEntity, bool>> predicate)
        {
            TEntity? entity = await _dbSet.FirstOrDefaultAsync(predicate);
            if(entity == null)
            {
                return null;
            }
            return entity;
            
        }
        public async Task<IEnumerable<TEntity>> GetAllEntitiesAsync()
        {
            var entities = await _dbSet.Where(e => e.IsDeleted == false).ToListAsync();
            return entities;
        }
        public async Task<TEntity> AddEntityAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateEntityAsync(TEntity entity)
        {
            var attach = _context.Attach<TEntity>(entity);
            IEnumerable<EntityEntry> unchangedEntities = _context.ChangeTracker.Entries().Where(x => x.State == EntityState.Unchanged);
            foreach(EntityEntry entityEntry in unchangedEntities)
            {
                entityEntry.State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }
        public async Task<ResultStatus> DeleteEntityByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if(entity != null)
            {
                bool hasDependencies = await HasDependenciesByIdAsync(id);
                if (!hasDependencies)
                {
                    return await NoDependencyCheckDeleteById(id); // not sure if FindAsync can be done twice in the same block
                }
                else
                {
                    return ResultStatus.NotAllowed;
                }
            }
            else
            {
                throw new BadHttpRequestException("Entity Not Found");
            }
        }
        public async Task<ResultStatus> NoDependencyCheckDeleteById(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if(entity is not null)
            {
                _context.Entry<TEntity>(entity).Property(e=>e.IsDeleted).CurrentValue = true;
                await _context.SaveChangesAsync();
                return ResultStatus.Success;
            }
            else
            {
                return ResultStatus.NotFound;
            }
        }

        public IEnumerable<PropertyInfo> GetNavigationProperties(int id)
        {
            var entityType = _context.Model.FindEntityType(typeof(TEntity));
            var collectionNavigations = entityType.GetNavigations()
                .Where(nav => nav.IsCollection)
                .Concat<INavigationBase>(entityType.GetSkipNavigations());
            var result = collectionNavigations.Select(r => r.PropertyInfo);
            return result;
        }
        public async Task<bool> HasDependenciesByIdAsync(int id)
        {
            TEntity entity = await GetEntityByIdAsync(id);
            if(entity is null)
            {
                throw new ExceptionEntityNotExists("Not Found");
            }
            IEnumerable<PropertyInfo> propertyInfoList = await Task.Run(() => GetNavigationProperties(id));
            bool hasDependencies = false;
            foreach(PropertyInfo propertyInfo in propertyInfoList)
            {
                hasDependencies = propertyInfo.GetValue(entity) is IEnumerable<BaseEntity> collection && collection.Any(c => c.IsDeleted == false);
                if (hasDependencies)
                {
                    break;
                }
            }
            return hasDependencies; 
        }

    }
}
