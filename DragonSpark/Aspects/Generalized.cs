using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;

namespace DragonSpark.Aspects
{
	public abstract class GeneralizedParameterizedSourceBase<TParameter, TResult> : SourceBase<TParameter, TResult>, ISpecification<TParameter>
	{
		[Specifications.Aspect]
		bool ISpecification<TParameter>.IsSatisfiedBy( TParameter parameter ) => false;
	}

	public abstract class SourceBase<TParameter, TResult> : ParameterizedSourceBase<TParameter, TResult>, IParameterizedSource<object, object>, ISpecification<object>
	{
		[Relay.SpecificationMethodAspect, Coercion.Aspect]
		bool ISpecification<object>.IsSatisfiedBy( object parameter ) => false;

		[Relay.ParameterizedSourceMethodAspect, Coercion.Aspect]
		object IParameterizedSource<object, object>.Get( object parameter ) => null;
	}

	public abstract class GeneralizedSpecificationBase<T> : SpecificationBase<T>, ISpecification<object>
	{
		[Relay.SpecificationMethodAspect, Coercion.Aspect]
		bool ISpecification<object>.IsSatisfiedBy( object parameter ) => false;
	}
}
