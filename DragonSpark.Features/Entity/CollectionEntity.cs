using System;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using DragonSpark.Objects;

namespace DragonSpark.Features.Entity
{
	[MetadataType( typeof(CollectionEntityMetadata) )]
	public class CollectionEntity
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }

		public Guid EntityWithCollectionId { get; set; }
	}

	class CollectionEntityMetadata
	{
		[Key, NewGuidDefaultValue]
		public object Id { get; set; }

		[DefaultPropertyValue( "New Basic Entity" )]
		public object Title { get; set; }
	}
}