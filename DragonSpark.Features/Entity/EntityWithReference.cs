using System;
using System.ComponentModel.DataAnnotations;
using DragonSpark.Features.Entity.Metadata;

namespace DragonSpark.Features.Entity
{
	[MetadataType( typeof(EntityWithReferenceMetadata) )]
	public class EntityWithReference
	{
		public Guid Id { get; set; }

		public string Title { get; set; }

		public ReferencedEntity ReferencedEntity { get; set; }

		public Guid? ReferencedEntityId { get; set; }
	}
}