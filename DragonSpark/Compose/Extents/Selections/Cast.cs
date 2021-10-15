using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Objects;
using System;

namespace DragonSpark.Compose.Extents.Selections;

public sealed class Cast<TIn, TOut> : Select<TIn, TOut>
{
	public static Cast<TIn, TOut> Default { get; } = new Cast<TIn, TOut>();

	Cast() : base(x => CastOrDefault<TIn, TOut>.Default.Get(x)) {}

	public Alternatives Or => Alternatives.Instance;

	public sealed class Alternatives
	{
		public static Alternatives Instance { get; } = new Alternatives();

		Alternatives() {}

		public Selector<TIn, TOut> Throw => CastOrDefault<TIn, TOut>.Default.Then();

		public Selector<TIn, TOut> Result => ResultAwareCast<TIn, TOut>.Default.Then();

		public Selector<TIn, TOut> Return(TOut result)
			=> new CastOrDefault<TIn, TOut>(new FixedResult<TIn, TOut>(result).Get).Then();

		public Selector<TIn, TOut> Return(Func<TOut> result)
			=> new CastOrDefault<TIn, TOut>(new DelegatedResult<TIn, TOut>(result).Get).Then();
	}
}