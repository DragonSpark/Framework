using System;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Aspects.Validation
{
	abstract class AdapterSourceBase<T> : CoercedParameterizedSource<T, IParameterValidationAdapter>
	{
		protected AdapterSourceBase( Func<T, IParameterValidationAdapter> create ) : base( create ) {}
	}
}