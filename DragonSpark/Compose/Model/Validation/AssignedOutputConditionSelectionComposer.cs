using DragonSpark.Model.Selection;

namespace DragonSpark.Compose.Model.Validation;

public sealed class AssignedOutputConditionSelectionComposer<TIn, TOut>
{
	public AssignedOutputConditionSelectionComposer(ISelect<TIn, TOut> otherwise)
		: this(new AssignedOutputOtherwiseComposer<TIn, TOut>(otherwise)) {}

	public AssignedOutputConditionSelectionComposer(AssignedOutputOtherwiseComposer<TIn, TOut> otherwise)
		=> Otherwise = otherwise;

	public AssignedOutputOtherwiseComposer<TIn, TOut> Otherwise { get; }
}