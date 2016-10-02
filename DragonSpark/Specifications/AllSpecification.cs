using System.Collections.Immutable;

namespace DragonSpark.Specifications
{
	public class AllSpecification : AllSpecification<object>
	{
		public AllSpecification( params ISpecification<object>[] specifications ) : base( specifications ) {}
	}

	public class AllSpecification<T> : SpecificationBase<T>
	{
		readonly ImmutableArray<ISpecification<T>> specifications;

		public AllSpecification( params ISpecification<T>[] specifications )
		{
			this.specifications = specifications.ToImmutableArray();
		}

		public override bool IsSatisfiedBy( T parameter )
		{
			foreach ( var specification in specifications )
			{
				if ( !specification.IsSatisfiedBy( parameter ) )
				{
					return false;
				}
			}
			return true;
		}
	}
}