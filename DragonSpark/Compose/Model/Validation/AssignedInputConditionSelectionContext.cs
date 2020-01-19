using DragonSpark.Model.Selection;

namespace DragonSpark.Compose.Model.Validation
{
	public sealed class AssignedInputConditionSelectionContext<TIn, TOut>
	{
		public AssignedInputConditionSelectionContext(ISelect<TIn, TOut> otherwise)
			: this(new AssignedInputOtherwiseContext<TIn, TOut>(otherwise)) {}

		public AssignedInputConditionSelectionContext(AssignedInputOtherwiseContext<TIn, TOut> otherwise)
			=> Otherwise = otherwise;

		public AssignedInputOtherwiseContext<TIn, TOut> Otherwise { get; }
	}
}