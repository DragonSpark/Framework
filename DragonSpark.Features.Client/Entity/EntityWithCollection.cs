using System.ComponentModel.DataAnnotations;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Application.Presentation.Entity;
using DragonSpark.Application.Presentation.Entity.Fields;
using DragonSpark.Objects;

namespace DragonSpark.Features.Entity
{
	[MetadataType( typeof(EntityWithCollectionMetadata) )]
	[RootEntitySet( DisplayNamePath = "Title", ItemName="Entity with Collection", ItemNamePlural="Entities with Collection", Title="Entities with Collection" )]
	public partial class EntityWithCollection
	{
		class EntityWithCollectionMetadata
		{
			[NewGuidDefaultValue]
			public object Id { get; set; }

			[DefaultPropertyValue( "New Entity with Collection" )]
			public object Title { get; set; }

			[IoCDefault, EntityFieldView( "Administration", ModelType = typeof(EntityCollectionField) ), EntityFieldView( IsViewable = false )]
			public object Entities { get; set; }
		}
	}
}