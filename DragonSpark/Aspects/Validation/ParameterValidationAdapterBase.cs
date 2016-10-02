using DragonSpark.Specifications;
using System;
using System.Reflection;

namespace DragonSpark.Aspects.Validation
{
	public abstract class ParameterValidationAdapterBase<T> : IParameterValidationAdapter
	{
		readonly ISpecification<T> inner;
		readonly ISpecification<object> fallback;
		readonly Func<MethodInfo, bool> method;

		protected ParameterValidationAdapterBase( ISpecification<T> inner, Func<MethodInfo, bool> method ) : this( inner, Common<object>.Never, method ) {}

		protected ParameterValidationAdapterBase( ISpecification<T> inner, ISpecification<object> fallback, Func<MethodInfo, bool> method )
		{
			this.inner = inner;
			this.fallback = fallback;
			this.method = method;
		}

		public bool IsSatisfiedBy( MethodInfo parameter ) => method( parameter );

		public bool IsSatisfiedBy( object parameter ) => parameter is T ? inner.IsSatisfiedBy( (T)parameter ) : fallback.IsSatisfiedBy( parameter );
	}
}