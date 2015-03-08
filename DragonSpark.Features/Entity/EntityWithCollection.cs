using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using DragonSpark.Features.Entity.Metadata;

namespace DragonSpark.Features.Entity
{
	[MetadataType( typeof(EntityWithCollectionMetadata) )]
	public class EntityWithCollection
	{
		public Guid Id { get; set; }

		public string Title { get; set; }

		public virtual List<CollectionEntity> Entities
		{
			get { return entities; }
		}	readonly List<CollectionEntity> entities = new List<CollectionEntity>();
	}
}