using DragonSpark.Specifications;
using System;

namespace DragonSpark.Sources.Parameterized
{
	public interface ISpecificationParameterizedSource<in TParameter, out TResult> : IParameterizedSource<TParameter, TResult>, ISpecification<TParameter> {}

	public class SpecificationParameterizedSource<TParameter, TResult> : DelegatedParameterizedSource<TParameter, TResult>, ISpecificationParameterizedSource<TParameter, TResult>
	{
		readonly ISpecification<TParameter> specification;

		public SpecificationParameterizedSource( ISpecification<TParameter> specification, IParameterizedSource<TParameter, TResult> source ) : this( specification, source.ToSourceDelegate() ) {}
		public SpecificationParameterizedSource( ISpecification<TParameter> specification, Func<TParameter, TResult> source ) : base( source )
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

		public bool IsSatisfiedBy( TParameter parameter ) => specification.IsSatisfiedBy( parameter );
	}
}