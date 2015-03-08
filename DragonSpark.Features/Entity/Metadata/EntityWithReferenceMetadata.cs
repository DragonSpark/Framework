using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using DragonSpark.Objects;

namespace DragonSpark.Features.Entity.Metadata
{
	public partial class EntityWithReferenceMetadata
	{
		[Key, NewGuidDefaultValue]
		public object Id { get; set; }

		[Include, Association( "ReferencedEntityEntityWithReference", "ReferencedEntityId", "Id", IsForeignKey = true )]
		[Display( Order = 10, Name = "Referenced Entity" )]
		public object ReferencedEntity { get; set; }
	}
}