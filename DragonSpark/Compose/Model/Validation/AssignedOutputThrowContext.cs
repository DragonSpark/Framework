using DragonSpark.Model;

namespace DragonSpark.Compose.Model.Validation;

sealed class AssignedOutputThrowContext<TIn, TOut> : OutputOtherwiseThrowContext<TIn, TOut>
{
	public AssignedOutputThrowContext(OutputOtherwiseContext<TIn, TOut> input)
		: base(input, x => new AssignedResultGuard<TIn>(Is.Always<TIn>().Out(), x)) {}
}