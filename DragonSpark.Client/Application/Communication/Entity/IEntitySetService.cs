using System.Collections.Generic;
using System.ServiceModel.DomainServices.Client;

namespace DragonSpark.Application.Communication.Entity
{
    public interface IEntitySetService
    {
        string GetQueryName( IEntitySetProfile descriptor, string view = null );

        IEnumerable<IEntitySetProfile> RetrieveProfiles();
        bool IsAuthorized( IEntitySetProfile descriptor, EntitySetOperations operation );
    }
}