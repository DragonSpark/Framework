using System;
using System.ServiceModel.DomainServices.Client;

namespace DragonSpark.Application.Presentation.Entity
{
    public interface IDomainContextLocator
    {
        DomainContext Locate( Type entityType );
    }
}