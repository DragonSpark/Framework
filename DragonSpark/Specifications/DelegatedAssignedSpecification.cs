using System;

namespace DragonSpark.Specifications
{
	public class DelegatedAssignedSpecification<TParameter, TResult> : SpecificationBase<TParameter>
	{
		readonly Func<TParameter, TResult> source;
		readonly Func<TResult, bool> specification;

		public DelegatedAssignedSpecification( Func<TParameter, TResult> source ) : this( source, AssignedSpecification<TResult>.Default.ToSpecificationDelegate() ) {}

		DelegatedAssignedSpecification( Func<TParameter, TResult> source, Func<TResult, bool> specification )
		{
			this.source = source;
			this.specification = specification;
		}

		public override bool IsSatisfiedBy( TParameter parameter ) => specification( source( parameter ) );
	}
}