using System;

namespace DragonSpark.Specifications
{
	public class DelegatedAssignedSpecification<TParameter, TResult> : SpecificationBase<TParameter>
	{
		readonly static Func<TResult, bool> Specification = AssignedSpecification<TResult>.Default.ToSpecificationDelegate();

		readonly Func<TParameter, TResult> source;
		readonly Func<TResult, bool> specification;

		public DelegatedAssignedSpecification( Func<TParameter, TResult> source ) : this( source, Specification ) {}

		DelegatedAssignedSpecification( Func<TParameter, TResult> source, Func<TResult, bool> specification )
		{
			this.source = source;
			this.specification = specification;
		}

		public override bool IsSatisfiedBy( TParameter parameter ) => specification( source( parameter ) );
	}
}