using DragonSpark.Application;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Composition
{
	sealed class ConventionImplementationLocator : AlterationBase<Type>
	{
		readonly static Func<ConventionMapping, bool> Where = IsValidConventionMappingSpecification.Default.ToSpecificationDelegate();

		readonly Func<ImmutableArray<Type>> source;
		readonly Func<ConventionMapping, bool> @where;

		public static ConventionImplementationLocator Default { get; } = new ConventionImplementationLocator();
		ConventionImplementationLocator() : this( ApplicationTypes.Default.ToDelegate(), Where ) {}

		public ConventionImplementationLocator( Func<ImmutableArray<Type>> source, Func<ConventionMapping, bool> @where )
		{
			this.source = source;
			this.@where = @where;
		}

		public override Type Get( Type parameter ) =>
			source()
				.Where( Activation.Defaults.Instantiable.And( TypeAssignableSpecification.Defaults.Get( parameter ) ).IsSatisfiedBy )
				.Introduce( parameter, tuple => new ConventionMapping( tuple.Item2, tuple.Item1 ) )
				.FirstOrDefault( @where ).ImplementationType;
	}
}