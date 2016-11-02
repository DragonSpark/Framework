using DragonSpark.Specifications;

namespace DragonSpark.Sources.Parameterized
{
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