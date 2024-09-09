using DragonSpark.Model;

namespace DragonSpark.Compose.Model.Validation;

sealed class AssignedOutputThrowComposer<TIn, TOut> : OutputOtherwiseThrowComposer<TIn, TOut>
{
	public AssignedOutputThrowComposer(OutputOtherwiseComposer<TIn, TOut> input)
		: base(input, x => new AssignedResultGuard<TIn>(Is.Always<TIn>().Out(), x)) {}
}