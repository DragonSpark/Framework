using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System;

namespace DragonSpark.Specifications
{
	public sealed class GenericTypeAssignableSpecification : DelegatedSpecification<Type>
	{
		public static IParameterizedSource<Type, ISpecification<Type>> Defaults { get; } = new Cache<Type, ISpecification<Type>>( type => new GenericTypeAssignableSpecification( type ).ToCachedSpecification() );
		GenericTypeAssignableSpecification( Type context ) : base( context.Adapt().IsGenericOf ) {}
	}
}