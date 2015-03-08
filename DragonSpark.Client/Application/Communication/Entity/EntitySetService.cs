using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel.DomainServices.Client;
using DragonSpark.Extensions;
using DragonSpark.IoC;

namespace DragonSpark.Application.Communication.Entity
{
	[Singleton( typeof(IEntitySetService), Priority = Priority.Lowest )]
	public class EntitySetService : IEntitySetService
	{
		readonly IPrincipal principal;
		readonly IEnumerable<IEntitySetProfile> descriptors;

		public EntitySetService( IPrincipal principal, IEnumerable<IEntitySetProfile> descriptors )
		{
			this.principal = principal;
			this.descriptors = descriptors;
		}

		public string GetQueryName( IEntitySetProfile descriptor, string view = null )
		{
			var queryNameAttributes = descriptor.EntityType.GetAttributes<QueryNameAttribute>();
			var first = queryNameAttributes.FirstOrDefault( x => x.ViewName == view ) ?? queryNameAttributes.FirstOrDefault( x => x.ViewName == null );
			var result = first.Transform( x => x.Name );
			return result;
		}

		public IEnumerable<IEntitySetProfile> RetrieveProfiles()
		{
			var result = descriptors.Where( x => x.AuthorizedRoles.All( principal.IsInRole ) ).ToList().AsReadOnly();
			return result;
		}

		public bool IsAuthorized( IEntitySetProfile descriptor, EntitySetOperations operation )
		{
			var operations = descriptor.Operations.Where( x => ( x.Operations & operation ) == operation );
			var result = principal.Identity.IsAuthenticated && operations.All( x => x.AuthorizedRoles.All( principal.IsInRole ) );
			return result;
		}
	}
}