using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System;

namespace DragonSpark.Specifications
{
	public static class TypeAssignableSpecification<T>
	{
		public static ISpecification<Type> Default { get; } = TypeAssignableSpecification.Defaults.Get( typeof(T) );
	}

	public sealed class TypeAssignableSpecification : DelegatedSpecification<Type>
	{
		public static IParameterizedSource<Type, ISpecification<Type>> Defaults { get; } = new Cache<Type, ISpecification<Type>>( type => new TypeAssignableSpecification( type ).ToCachedSpecification() );
		TypeAssignableSpecification( Type targetType ) : base( targetType.Adapt().IsAssignableFrom ) {}
	}
}