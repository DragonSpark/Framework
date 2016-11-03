using DragonSpark.Coercion;
using System;

namespace DragonSpark.Aspects.Validation
{
	abstract class AdapterSourceBase<T> : LinkedParameterizedSource<T, IParameterValidationAdapter>
	{
		protected AdapterSourceBase( Func<T, IParameterValidationAdapter> create ) : base( create ) {}
	}
}