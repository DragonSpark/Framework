using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System.Runtime.InteropServices;

namespace DragonSpark.Sources.Coercion
{
	public abstract class CoercerBase<T> : CoercerBase<object, T>, IParameterizedSource<T> {}

	public abstract class CoercerBase<TFrom, TTo> : IParameterizedSource<TFrom, TTo>
	{
		public TTo Get( [Optional]TFrom parameter ) => parameter is TTo ? parameter.To<TTo>() : parameter.IsAssigned() ? Coerce( parameter ) : default(TTo);

		protected abstract TTo Coerce( TFrom parameter );
	}

	public sealed class CastCoercer<TFrom, TTo> : CoercerBase<TFrom, TTo>
	{
		public static CastCoercer<TFrom, TTo> Default { get; } = new CastCoercer<TFrom, TTo>();
		CastCoercer() {}

		protected override TTo Coerce( TFrom parameter ) => parameter.AsValid<TTo>();
	}
}