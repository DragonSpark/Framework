using DragonSpark.Model;

namespace DragonSpark.Compose.Model.Validation;

sealed class AssignedInputThrowContext<TIn, TOut> : InputOtherwiseThrowContext<TIn, TOut>
{
	public AssignedInputThrowContext(InputOtherwiseContext<TIn, TOut> input)
		: base(input, x => new AssignedEntryGuard<TIn>(x)) {}
}