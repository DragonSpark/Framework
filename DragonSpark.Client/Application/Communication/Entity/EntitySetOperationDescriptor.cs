using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel.DomainServices.Client;

namespace DragonSpark.Application.Communication.Entity
{
	public class EntitySetOperationDescriptor : IEntitySetOperationProfile
	{
		public EntitySetOperations Operations { get; set; }

		[TypeConverter( typeof(StringArrayConverter) )]
		public IEnumerable<string> AuthorizedRoles { get; set; }
	}
}