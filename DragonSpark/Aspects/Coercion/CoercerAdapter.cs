using DragonSpark.Coercion;

namespace DragonSpark.Aspects.Coercion
{
	sealed class CoercerAdapter<TFrom, TTo> : ICoercer
	{
		readonly ICoercer<TFrom, TTo> coercer;

		public CoercerAdapter( ICoercer<TFrom, TTo> coercer )
		{
			this.coercer = coercer;
		}

		public object Coerce( object parameter ) => parameter is TFrom ? (object)coercer.Coerce( (TFrom)parameter ) ?? parameter : parameter;
	}
}