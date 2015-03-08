using System.ComponentModel.DataAnnotations;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Application.Presentation.Entity;
using DragonSpark.Application.Presentation.Entity.Fields;
using DragonSpark.Objects;

namespace DragonSpark.Features.Entity
{
	[MetadataType( typeof(EntityWithReferenceMetadata) )]
	[RootEntitySet( DisplayNamePath = "Title", ItemName="Entity with Reference", ItemNamePlural="Entities with Reference", Title="Entities with Reference" )]
	public partial class EntityWithReference
	{
		class EntityWithReferenceMetadata
		{
			[NewGuidDefaultValue]
			public object Id { get; set; }

			[DefaultPropertyValue( "New Entity with Reference" )]
			public object Title { get; set; }

			[IoCDefault, EntityFieldView( "Administration", ModelType = typeof(EntityReferenceField) ), EntityFieldView( IsViewable = false )]
			public object ReferencedEntity { get; set; }
		}
	}
}