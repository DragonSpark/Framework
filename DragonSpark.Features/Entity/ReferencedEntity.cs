using System;
using System.ComponentModel.DataAnnotations;
using DragonSpark.Objects;

namespace DragonSpark.Features.Entity
{
	[MetadataType( typeof(ReferencedEntityMetadata) )]
	public class ReferencedEntity
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
	}
	
	class ReferencedEntityMetadata
	{
		[Key, NewGuidDefaultValue]
		public object Id { get; set; }

		[DefaultPropertyValue( "New Basic Entity" )]
		public object Title { get; set; }
	}
}