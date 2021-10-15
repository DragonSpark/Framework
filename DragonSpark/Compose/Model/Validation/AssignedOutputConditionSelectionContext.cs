using DragonSpark.Model.Selection;

namespace DragonSpark.Compose.Model.Validation;

public sealed class AssignedOutputConditionSelectionContext<TIn, TOut>
{
	public AssignedOutputConditionSelectionContext(ISelect<TIn, TOut> otherwise)
		: this(new AssignedOutputOtherwiseContext<TIn, TOut>(otherwise)) {}

	public AssignedOutputConditionSelectionContext(AssignedOutputOtherwiseContext<TIn, TOut> otherwise)
		=> Otherwise = otherwise;

	public AssignedOutputOtherwiseContext<TIn, TOut> Otherwise { get; }
}