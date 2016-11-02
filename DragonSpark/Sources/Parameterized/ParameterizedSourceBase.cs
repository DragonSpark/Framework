using DragonSpark.Specifications;
using System;

namespace DragonSpark.Sources.Parameterized
{
	public abstract class ParameterizedSourceBase<T> : ParameterizedSourceBase<object, T>, IParameterizedSource<T> {}

	public abstract class ParameterizedSourceBase<TParameter, TResult> : IParameterizedSource<TParameter, TResult>
	{
		public abstract TResult Get( TParameter parameter );
	}

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

	public class ConditionalInstanceParameterizedSource<TParameter, TResult> : ParameterizedSourceBase<TParameter, TResult>
	{
		readonly ISpecification<TParameter> specification;
		readonly TResult satisfied, unsatisfied;

		public ConditionalInstanceParameterizedSource( ISpecification<TParameter> specification, TResult satisfied, TResult unsatisfied )
		{
			this.specification = specification;
			this.satisfied = satisfied;
			this.unsatisfied = unsatisfied;
		}

		public override TResult Get( TParameter parameter ) => specification.IsSatisfiedBy( parameter ) ? satisfied : unsatisfied;
	}
}