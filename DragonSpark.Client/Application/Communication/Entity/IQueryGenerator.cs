using System;
using System.Collections.Generic;
using System.ServiceModel.DomainServices.Client;

namespace DragonSpark.Application.Communication.Entity
{
    public interface IQueryGenerator
    {
        EntityQuery Generate( Type entityType, DomainContext context, string methodName = null, IEnumerable<object> parameters = null, params IQueryShaper[] shapers );
    }
}