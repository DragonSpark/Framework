using System.ServiceModel.DomainServices.Client;

namespace DragonSpark.Application.Communication.Entity
{
    public interface IQueryShaper
    {
        EntityQuery<TEntity> Shape<TEntity>( EntityQuery<TEntity> query ) where TEntity : System.ServiceModel.DomainServices.Client.Entity;
    }
}