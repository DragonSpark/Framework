using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DragonSpark.Sources;

namespace DragonSpark.Composition
{
	public sealed class ExportMappings : Scope<ImmutableArray<ExportMapping>>
	{
		public static ExportMappings Default { get; } = new ExportMappings();
		ExportMappings() : base( () => ExportSource<IEnumerable<ExportMapping>>.Default.GetEnumerable().Concat().ToImmutableArray() ) {}
	}
}