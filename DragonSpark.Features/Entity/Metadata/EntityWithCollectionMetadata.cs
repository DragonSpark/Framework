using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using DragonSpark.Objects;

namespace DragonSpark.Features.Entity.Metadata
{
	public partial class EntityWithCollectionMetadata
	{
		[Key, NewGuidDefaultValue]
		public object Id { get; set; }

		[Include, Association( "CollectionEntityEntityWithCollection", "Id", "EntityWithCollectionId" ), Composition]
		public object Entities { get; set; }
	}
}