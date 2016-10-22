using DragonSpark.Sources;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Composition
{
	public sealed class ExportMappings : Scope<ImmutableArray<ExportMapping>>
	{
		public static ExportMappings Default { get; } = new ExportMappings();
		ExportMappings() : base( () => Sources.Extensions.AsEnumerable( ExportSource<IEnumerable<ExportMapping>>.Default ).Concat().ToImmutableArray() ) {}
	}
}