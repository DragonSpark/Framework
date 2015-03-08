using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Microsoft.Practices.ServiceLocation;

namespace Common.EntityModel
{
	public class EntityServiceResolver : IEntityServiceResolver
	{
		readonly static Dictionary<Type,IEntityService> Cache = new Dictionary<Type, IEntityService>();
		readonly IServiceLocator locator;

		public EntityServiceResolver( IServiceLocator locator )
		{
			this.locator = locator;
		}

		public IEntityService Resolve( Type entityType )
		{
			var result = Cache.Ensure( entityType, ResolveService );
			return result;
		}

		IEntityService ResolveService( Type entityType )
		{
			var query = from pair in AppDomain.CurrentDomain.GetAllTypesWith<EntityServiceAttribute>()
			            where pair.Key.EntityType == entityType
			            let service = locator.GetInstance( pair.Value )
			            select service is IEntityService ? service.To<IEntityService>() : new EntityServiceProxy( pair.Value, service );
			var result = query.SingleOrDefault();
			return result;
		}
	}
}