using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Runtime.Invocation
{
	sealed class StripedAlteration<TIn, TOut> : Alteration<ISelect<TIn, TOut>> where TIn : notnull
	{
		public static StripedAlteration<TIn, TOut> Default { get; } = new StripedAlteration<TIn, TOut>();

		StripedAlteration() : base(Start.An.Extent<Striped<TIn, TOut>>().From) {}
	}
}