using System.Collections.Immutable;

namespace DragonSpark.Specifications
{
	public class AnySpecification<T> : SpecificationBase<T>
	{
		readonly ImmutableArray<ISpecification<T>> specifications;

		public AnySpecification( params ISpecification<T>[] specifications )
		{
			this.specifications = specifications.ToImmutableArray();
		}

		public override bool IsSatisfiedBy( T parameter )
		{
			foreach ( var specification in specifications )
			{
				if ( specification.IsSatisfiedBy( parameter ) )
				{
					return true;
				}
			}
			return false;
		}
	}
}