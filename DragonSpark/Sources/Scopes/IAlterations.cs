using DragonSpark.Sources.Parameterized;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace DragonSpark.Sources.Scopes
{
	[SuppressMessage( "ReSharper", "PossibleInfiniteInheritance" )]
	public interface IAlterations<T> : IScope<ImmutableArray<IAlteration<T>>> {}
}