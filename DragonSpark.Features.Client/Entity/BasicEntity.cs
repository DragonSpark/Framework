using System.ComponentModel.DataAnnotations;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Objects;

namespace DragonSpark.Features.Entity
{
	[MetadataType( typeof(BasicEntityMetadata) )]
	[RootEntitySet( DisplayNamePath = "Title", ItemName="Basic Entity", ItemNamePlural="Basic Entities", Title="Basic Entities" )]
	public partial class BasicEntity
	{
		class BasicEntityMetadata
		{
			[NewGuidDefaultValue]
			public object Id { get; set; }

			[DefaultPropertyValue( "New Basic Entity" )]
			public object Title { get; set; }
		}
	}
}