using System;
using DragonSpark.Specifications;

namespace DragonSpark.Sources.Parameterized
{
	public class ConditionalParameterizedSource<TParameter, TResult> : ParameterizedSourceBase<TParameter, TResult>
	{
		readonly ISpecification<TParameter> specification;
		readonly Func<TParameter, TResult> satisfiedSource;
		readonly Func<TParameter, TResult> unsatisfiedSource;

		public ConditionalParameterizedSource( ISpecification<TParameter> specification, Func<TParameter, TResult> satisfiedSource, Func<TParameter, TResult> unsatisfiedSource )
		{
			this.specification = specification;
			this.satisfiedSource = satisfiedSource;
			this.unsatisfiedSource = unsatisfiedSource;
		}

		public override TResult Get( TParameter parameter ) => specification.IsSatisfiedBy( parameter ) ? satisfiedSource( parameter ) : unsatisfiedSource( parameter );
	}
}