using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Composition
{
	public sealed class ConventionImplementations : CompositeFactory<Type, Type>
	{
		public static IParameterizedSource<Type, Type> Default { get; } = new ParameterizedScope<Type, Type>( new ConventionImplementations().Apply( Defaults.ConventionCandidate ).ToSourceDelegate().GlobalCache() );
		ConventionImplementations() : base( MappedConventionLocator.Default, ConventionImplementationLocator.Default ) {}
	}
}