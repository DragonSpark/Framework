using DragonSpark.Sources.Scopes;
using System.Collections.Immutable;

namespace DragonSpark.TypeSystem
{
	public sealed class AssemblyResourcePathSelector : ParameterizedScopedSingleton<string, ImmutableArray<string>>
	{
		public static AssemblyResourcePathSelector Default { get; } = new AssemblyResourcePathSelector();
		AssemblyResourcePathSelector() : base( path => Items<string>.Immutable ) {}
	}
}