using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Reflection;

namespace DragonSpark.Runtime.Invocation
{
	sealed class StripedAlteration<TIn, TOut> : Alteration<ISelect<TIn, TOut>>
	{
		public static StripedAlteration<TIn, TOut> Default { get; } = new StripedAlteration<TIn, TOut>();

		StripedAlteration() : base(I<Striped<TIn, TOut>>.Default.From) {}
	}
}