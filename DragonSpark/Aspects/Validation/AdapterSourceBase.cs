using DragonSpark.Sources.Coercion;
using System;

namespace DragonSpark.Aspects.Validation
{
	abstract class AdapterSourceBase<T> : ParameterCoercionSource<T, IParameterValidationAdapter>
	{
		protected AdapterSourceBase( Func<T, IParameterValidationAdapter> create ) : base( create ) {}
	}
}