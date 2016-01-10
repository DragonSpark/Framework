using DragonSpark.Extensions;

namespace DragonSpark.Runtime.Specifications
{
	public abstract class SpecificationBase<TParameter> : ISpecification<TParameter>
	{
		bool ISpecification.IsSatisfiedBy( object parameter ) => IsSatisfiedBy( parameter is TParameter ? (TParameter)parameter : default(TParameter) );

		public bool IsSatisfiedBy( TParameter parameter ) => IsSatisfiedByParameter( parameter );

		protected virtual bool IsSatisfiedByParameter( TParameter parameter ) => !parameter.IsNull();
	}
}