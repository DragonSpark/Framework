using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Reflection;

namespace DragonSpark.Runtime.Invocation
{
	sealed class ProtectAlteration<TIn, TOut> : Alteration<ISelect<TIn, TOut>>
	{
		public static ProtectAlteration<TIn, TOut> Default { get; } = new ProtectAlteration<TIn, TOut>();

		ProtectAlteration() : base(I<Protect<TIn, TOut>>.Default.From) {}
	}
}