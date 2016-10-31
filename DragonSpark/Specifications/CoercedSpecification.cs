using DragonSpark.Extensions;
using System;

namespace DragonSpark.Specifications
{
	public class CoercedSpecification<TFrom, TTo> : SpecificationBase<TFrom>, ISpecification<TTo>
	{
		readonly Func<TFrom, TTo> coerce;
		readonly Func<TTo, bool> specification;

		public CoercedSpecification( Func<TFrom, TTo> coerce, Func<TTo, bool> specification )
		{
			this.coerce = coerce;
			this.specification = specification;
		}

		public bool IsSatisfiedBy( TTo parameter ) => specification( parameter );

		public override bool IsSatisfiedBy( TFrom parameter )
		{
			var to = coerce( parameter );
			var result = to.IsAssigned() && IsSatisfiedBy( to );
			return result;
		}
	}
}