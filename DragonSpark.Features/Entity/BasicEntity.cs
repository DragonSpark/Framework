using System;
using System.ComponentModel.DataAnnotations;
using DragonSpark.Objects;

namespace DragonSpark.Features.Entity
{
	[MetadataType( typeof(BasicEntityMetadata) )]
	public class BasicEntity
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string BasicStringProperty { get; set; }
		public string AnotherStringProperty { get; set; }
		public string AnotherStringProperty2 { get; set; }
	}

	class BasicEntityMetadata
	{
		[Key, NewGuidDefaultValue]
		public object Id { get; set; }

		[DefaultPropertyValue( "New Basic Entity" )]
		public object Title { get; set; }
	}
}