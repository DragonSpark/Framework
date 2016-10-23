using DragonSpark.Aspects.Validation;
using DragonSpark.Specifications;

namespace DragonSpark.Sources.Parameterized
{
	sealed class AutoValidatingSource<TParameter, TResult> : AutoValidatingSourceBase<TParameter, TResult>, IParameterizedSource<TParameter, TResult>, ISpecification<TParameter>
	{
		public AutoValidatingSource( ISpecification<TParameter> specification, IParameterizedSource<TParameter, TResult> inner ) : 
			base( new AutoValidationController( new Aspects.Validation.SourceAdapter<TParameter, TResult>( specification ) ), specification.IsSatisfiedBy, inner.Get )
		{}
	}
}