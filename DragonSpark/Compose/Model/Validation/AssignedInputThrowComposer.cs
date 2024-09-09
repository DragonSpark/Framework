using DragonSpark.Model;

namespace DragonSpark.Compose.Model.Validation;

sealed class AssignedInputThrowComposer<TIn, TOut> : InputOtherwiseThrowComposer<TIn, TOut>
{
	public AssignedInputThrowComposer(InputOtherwiseComposer<TIn, TOut> input)
		: base(input, x => new AssignedEntryGuard<TIn>(x)) {}
}