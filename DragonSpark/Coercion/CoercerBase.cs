using DragonSpark.Extensions;

namespace DragonSpark.Coercion
{
	public abstract class CoercerBase<T> : CoercerBase<object, T>, ICoercer<T> {}

	public abstract class CoercerBase<TFrom, TTo> : ICoercer<TFrom, TTo>
	{
		public TTo Coerce( TFrom parameter ) => parameter is TTo ? parameter.To<TTo>() : parameter.IsAssignedOrValue() ? Apply( parameter ) : default(TTo);

		protected abstract TTo Apply( TFrom parameter );
	}

	public class Coercer<TFrom, TTo> : CoercerBase<TFrom, TTo>
	{
		public static Coercer<TFrom, TTo> Default { get; } = new Coercer<TFrom, TTo>();
		Coercer() {}

		protected override TTo Apply( TFrom parameter ) => default(TTo);
	}
}