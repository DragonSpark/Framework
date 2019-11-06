using System;
using DragonSpark.Runtime.Objects;

namespace DragonSpark.Model.Selection
{
	sealed class DecorationSelector<TFrom, TTo, TOut> : ISelect<Decoration<TFrom, TOut>, Decoration<TTo, TOut>>
	{
		public static DecorationSelector<TFrom, TTo, TOut> Default { get; } =
			new DecorationSelector<TFrom, TTo, TOut>();

		DecorationSelector() : this(CastOrThrow<TFrom, TTo>.Default.Get) {}

		readonly Func<TFrom, TTo> _select;

		public DecorationSelector(Func<TFrom, TTo> coercer) => _select = coercer;

		public Decoration<TTo, TOut> Get(Decoration<TFrom, TOut> parameter)
			=> new Decoration<TTo, TOut>(_select(parameter.Parameter), parameter.Result);
	}
}