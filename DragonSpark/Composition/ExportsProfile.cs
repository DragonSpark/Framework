using DragonSpark.Specifications;
using System;

namespace DragonSpark.Composition
{
	public struct ExportsProfile : ISpecification<Type>
	{
		readonly ISpecification<Type> specification;

		public ExportsProfile( ConstructedExports constructed, ConventionExports convention, SingletonExports singletons, ISpecification<Type> specification )
		{
			this.specification = specification;
			Constructed = constructed;
			Convention = convention;
			Singletons = singletons;
		}

		public ConstructedExports Constructed { get; }
		public ConventionExports Convention { get; }
		public SingletonExports Singletons { get; }

		public bool IsSatisfiedBy( Type parameter ) => specification.IsSatisfiedBy( parameter );
	}
}