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
		bool ISpecification.IsSatisfiedBy( object parameter ) => IsSatisfiedBy( parameter is TParameter ? (TParameter)parameter : default(TParameter) );

		public bool IsSatisfiedBy( TParameter parameter ) => IsSatisfiedByParameter( parameter );

		protected virtual bool IsSatisfiedByParameter( TParameter parameter ) => !parameter.IsNull();
	}
}