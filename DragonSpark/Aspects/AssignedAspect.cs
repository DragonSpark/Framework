using DragonSpark.Runtime;

namespace DragonSpark.Aspects
{
	public sealed class AssignedAspect<TIn, TOut> : ValidationAspect<TIn, TOut>
	{
		public static AssignedAspect<TIn, TOut> Default { get; } = new AssignedAspect<TIn, TOut>();

		AssignedAspect() : base(DefaultGuard<TIn>.Default.Execute) {}
	}
}