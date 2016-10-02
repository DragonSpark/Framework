using System;
using System.Reflection;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;

namespace DragonSpark.TypeSystem.Generics
{
	sealed class GenericMethodEqualitySpecification : SpecificationWithContextBase<Type[], MethodBase>
	{
		public static ICache<MethodBase, ISpecification<Type[]>> Default { get; } = new Cache<MethodBase, ISpecification<Type[]>>( method => new GenericMethodEqualitySpecification( method ) );
		GenericMethodEqualitySpecification( MethodBase method ) : base( method ) {}

		public override bool IsSatisfiedBy( Type[] parameter ) => Context.GetGenericArguments().Length == parameter.Length;
	}
}