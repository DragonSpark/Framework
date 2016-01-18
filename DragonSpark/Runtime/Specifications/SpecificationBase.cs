using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Runtime.Specifications
{
	public class InverseSpecification : ISpecification
	{
		readonly ISpecification inner;

		public InverseSpecification( [Required]ISpecification inner )
		{
			this.inner = inner;
		}

		public bool IsSatisfiedBy( object context ) => !inner.IsSatisfiedBy( context );
	}

	public abstract class SpecificationBase<TParameter> : ISpecification<TParameter>
	{
		bool ISpecification.IsSatisfiedBy( object parameter ) => parameter is TParameter && IsSatisfiedBy( (TParameter)parameter );

		public bool IsSatisfiedBy( TParameter parameter ) => Verify( parameter );

		protected abstract bool Verify( TParameter parameter );
	}
}