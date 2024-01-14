using DemoToListBE.Data.Entity;

namespace DemoToListBE.Data.DomainModel
{
    public class BaseDomainModel<TEntity> where TEntity : BaseEntity
    {
        public TEntity _entity;
        public BaseDomainModel(TEntity entity)
        {
            _entity = entity;
        }
        public BaseDomainModel()
        {

        }
    }
}
