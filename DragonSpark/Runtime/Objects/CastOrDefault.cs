using System;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime.Objects
{
	sealed class CastOrDefault<TFrom, TTo> : ISelect<TFrom, TTo>
	{
		public static CastOrDefault<TFrom, TTo> Default { get; } = new CastOrDefault<TFrom, TTo>();

		CastOrDefault() : this(Start.A.Selection<TFrom>().By.Default<TTo>().Get) {}

		readonly Func<TFrom, TTo> _default;

		public CastOrDefault(Func<TFrom, TTo> @default) => _default = @default;

		public TTo Get(TFrom parameter) => parameter is TTo to ? to : _default(parameter);
	}
}