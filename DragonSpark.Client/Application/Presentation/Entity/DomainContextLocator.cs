using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Extensions;
using DragonSpark.IoC;

namespace DragonSpark.Application.Presentation.Entity
{
    [Singleton( typeof(IDomainContextLocator), Priority = Priority.Lowest )]
    public class DomainContextLocator : IDomainContextLocator
    {
        readonly IEnumerable<DomainContext> contexts;

        public DomainContextLocator( IEnumerable<DomainContext> contexts )
        {
            this.contexts = contexts;
        }

        public DomainContext Locate( Type entityType )
        {
            var items = contexts.Transform( x => Enumerable.Except<DomainContext>( x, Enumerable.OfType<AuthenticationDomainContextBase>( x ) ).ToArray() );
            var result = items.FirstOrDefault( x => x.DetermineEntitySet( entityType ) != null );
            return result;
        }
    }
}