using DragonSpark.Specifications;
using System;

namespace DragonSpark.Sources.Parameterized
{
	public class SpecificationParameterizedSource<TParameter, TResult> : DelegatedParameterizedSource<TParameter, TResult>, ISpecificationParameterizedSource<TParameter, TResult>
	{
		readonly Func<TParameter, bool> specification;

		public SpecificationParameterizedSource( ISpecification<TParameter> specification, Func<TParameter, TResult> second ) : this( specification.ToSpecificationDelegate(), second ) {}

		public SpecificationParameterizedSource( Func<TParameter, bool> specification, Func<TParameter, TResult> second ) : base( second )
		{
			this.specification = specification;
		}

		public override TResult Get( TParameter parameter )
		{
			var validated = Validate( parameter );
			var result = validated ? base.Get( parameter ) : default(TResult);
			return result;
		}

		protected virtual bool Validate( TParameter parameter ) => IsSatisfiedBy( parameter );

		public bool IsSatisfiedBy( TParameter parameter ) => specification( parameter );
	}
}