using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Aspects.Coercion
{
	sealed class CoercerAdapter<TFrom, TTo> : ICoercer
	{
		readonly IParameterizedSource<TFrom, TTo> coercer;

		public CoercerAdapter( IParameterizedSource<TFrom, TTo> coercer )
		{
			this.coercer = coercer;
		}

		public object Coerce( object parameter ) => parameter is TFrom ? (object)coercer.Get( (TFrom)parameter ) ?? parameter : parameter;
	}
}