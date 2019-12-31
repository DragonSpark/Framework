using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Runtime.Invocation
{
	sealed class ProtectAlteration<TIn, TOut> : Alteration<ISelect<TIn, TOut>>
	{
		public static ProtectAlteration<TIn, TOut> Default { get; } = new ProtectAlteration<TIn, TOut>();

		ProtectAlteration() : base(Start.An.Extent<Protect<TIn, TOut>>().From) {}
	}
}