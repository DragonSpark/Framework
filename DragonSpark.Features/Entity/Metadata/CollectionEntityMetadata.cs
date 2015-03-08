using System.ComponentModel.DataAnnotations;
using DragonSpark.Objects;

namespace DragonSpark.Features.Entity.Metadata
{
	public partial class CollectionEntityMetadata
	{
		[Key, NewGuidDefaultValue]
		public object Id { get; set; }

		[DefaultPropertyValue( "New Basic Entity" )]
		public object Title { get; set; }
	}
}