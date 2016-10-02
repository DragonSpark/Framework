using System;

namespace DragonSpark.Specifications
{
	public sealed class DeferredSpecification<T> : SpecificationBase<T>
	{
		readonly Func<ISpecification<T>> source;
		public DeferredSpecification( Func<ISpecification<T>> source )
		{
			this.source = source;
		}

		public override bool IsSatisfiedBy( T parameter ) => source().IsSatisfiedBy( parameter );
	}
}