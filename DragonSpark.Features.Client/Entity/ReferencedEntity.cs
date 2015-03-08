using System.ComponentModel.DataAnnotations;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Objects;

namespace DragonSpark.Features.Entity
{
	[MetadataType( typeof(ReferencedEntityMetadata) )]
	[EntitySet( DisplayNamePath = "Title", ItemName="Referenced Entity", ItemNamePlural="Referenced Entities", Title="Referenced Entities" )]
	public partial class ReferencedEntity
	{
		class ReferencedEntityMetadata
		{
			[NewGuidDefaultValue]
			public object Id { get; set; }

			[DefaultPropertyValue( "New Referenced Entity" )]
			public object Title { get; set; }
		}
	}

	[MetadataType( typeof(CollectionEntityMetadata) )]
	[EntitySet( DisplayNamePath = "Title", ItemName="Collection Entity", ItemNamePlural="Collection Entities", Title="Collection Entities" )]
	public partial class CollectionEntity
	{
		class CollectionEntityMetadata
		{
			[NewGuidDefaultValue]
			public object Id { get; set; }

			[DefaultPropertyValue( "New Collection Entity" )]
			public object Title { get; set; }
		}
	}
}