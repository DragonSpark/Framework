using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Configuration
{
	[SuppressMessage( "ReSharper", "PossibleInfiniteInheritance" )]
	public interface IConfigurationScope<T> : IScope<ImmutableArray<IAlteration<T>>> {}
}