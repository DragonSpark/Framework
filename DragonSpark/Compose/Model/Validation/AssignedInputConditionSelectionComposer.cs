using DragonSpark.Model.Selection;

namespace DragonSpark.Compose.Model.Validation;

public sealed class AssignedInputConditionSelectionComposer<TIn, TOut>
{
	public AssignedInputConditionSelectionComposer(ISelect<TIn, TOut> otherwise)
		: this(new AssignedInputOtherwiseComposer<TIn, TOut>(otherwise)) {}

	public AssignedInputConditionSelectionComposer(AssignedInputOtherwiseComposer<TIn, TOut> otherwise)
		=> Otherwise = otherwise;

	public AssignedInputOtherwiseComposer<TIn, TOut> Otherwise { get; }
}