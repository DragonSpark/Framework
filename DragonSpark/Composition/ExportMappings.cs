using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Composition
{
	public sealed class ExportMappings : SingletonScope<ImmutableArray<ExportMapping>>
	{
		public static ExportMappings Default { get; } = new ExportMappings();
		ExportMappings() : base( () => ExportSource<IEnumerable<ExportMapping>>.Default.Concat().ToImmutableArray() ) {}
	}
}