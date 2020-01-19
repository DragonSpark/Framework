using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Validation
{
	public sealed class InputConditionSelectionContext<TIn, TOut>
	{
		public InputConditionSelectionContext(ISelect<TIn, TOut> subject, Func<TIn, bool> condition)
			: this(new OtherwiseThrowInputContext<TIn, TOut>(subject, condition)) {}

		public InputConditionSelectionContext(OtherwiseThrowInputContext<TIn, TOut> otherwise)
			=> Otherwise = otherwise;

		public OtherwiseThrowInputContext<TIn, TOut> Otherwise { get; }
	}
}