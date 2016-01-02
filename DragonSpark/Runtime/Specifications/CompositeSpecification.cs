using System;

namespace DragonSpark.Runtime.Specifications
{
	public abstract class CompositeSpecification : ISpecification
	{
		readonly Func<Func<ISpecification, bool>, bool> @where;
		
		protected CompositeSpecification( Func<Func<ISpecification, bool>, bool> where )
		{
			this.@where = @where;
		}

		public bool IsSatisfiedBy( object context )
		{
			var result = where( condition => condition.IsSatisfiedBy( context ) );
			return result;
		}
	}
}