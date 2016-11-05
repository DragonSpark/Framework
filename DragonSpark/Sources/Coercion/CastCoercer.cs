using DragonSpark.Extensions;

namespace DragonSpark.Sources.Coercion
{
	public sealed class CastCoercer<TFrom, TTo> : CoercerBase<TFrom, TTo>
	{
		public static CastCoercer<TFrom, TTo> Default { get; } = new CastCoercer<TFrom, TTo>();
		CastCoercer() {}

		protected override TTo Coerce( TFrom parameter ) => parameter.AsValid<TTo>();
	}
}