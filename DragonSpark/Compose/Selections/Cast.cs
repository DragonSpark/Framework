using System;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Objects;

namespace DragonSpark.Compose.Selections
{
	public sealed class Cast<TIn, TOut> : Select<TIn, TOut>
	{
		public static Cast<TIn, TOut> Default { get; } = new Cast<TIn, TOut>();

		Cast() : base(x => CastOrDefault<TIn, TOut>.Default.Get(x)) {}

		public Alternatives Or => Alternatives.Instance;

		public sealed class Alternatives
		{
			public static Alternatives Instance { get; } = new Alternatives();

			Alternatives() {}

			public ISelect<TIn, TOut> Throw => CastOrDefault<TIn, TOut>.Default;

			public ISelect<TIn, TOut> Result => ResultAwareCast<TIn, TOut>.Default;

			public ISelect<TIn, TOut> Return(TOut result)
				=> new CastOrDefault<TIn, TOut>(new FixedResult<TIn, TOut>(result).Get);

			public ISelect<TIn, TOut> Return(Func<TOut> result)
				=> new CastOrDefault<TIn, TOut>(new DelegatedResult<TIn, TOut>(result).Get);
		}
	}
}