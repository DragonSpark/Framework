using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using System;

namespace DragonSpark.Composition
{
	public sealed class ConventionImplementations : CompositeFactory<Type, Type>
	{
		public static IParameterizedSource<Type, Type> Default { get; } = new ConventionImplementations().Apply( Defaults.ConventionCandidate ).ToSingletonScope();
		ConventionImplementations() : base( MappedConventionLocator.Default, ConventionImplementationLocator.Default ) {}
	}
}