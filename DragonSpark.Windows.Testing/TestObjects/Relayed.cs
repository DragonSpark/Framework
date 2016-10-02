using DragonSpark.Testing.Objects;
using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Windows.Testing.TestObjects
{
	[MetadataType( typeof(RelayedDescriptor) )]
	class Relayed
	{
		[Attribute( PropertyName = "This is a relayed class attribute." )]
		class RelayedDescriptor
		{
			[Attribute( PropertyName = "This is a relayed property attribute." ), UsedImplicitly]
			public string Property { get; set; }
		}

		public string Property { get; set; }
	}
}