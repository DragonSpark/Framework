using System.Collections.Immutable;
using DragonSpark.Sources.Scopes;

namespace DragonSpark.TypeSystem
{
	public sealed class AssemblyResourcePathSelector : ConfigurableParameterizedSource<string, ImmutableArray<string>>
	{
		public static AssemblyResourcePathSelector Default { get; } = new AssemblyResourcePathSelector();
		AssemblyResourcePathSelector() : base( path => Items<string>.Immutable ) {}
	}
}