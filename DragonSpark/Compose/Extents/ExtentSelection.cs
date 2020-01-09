using DragonSpark.Compose.Extents.Results;

namespace DragonSpark.Compose.Extents
{
	public sealed class ExtentSelection<T>
	{
		public static ExtentSelection<T> Default { get; } = new ExtentSelection<T>();

		ExtentSelection() {}

		public Context<T> Result => Context<T>.Instance;

		public Conditions.ConditionContext<T> Condition => Conditions.ConditionContext<T>.Instance;

		public Commands.CommandContext<T> Command => Commands.CommandContext<T>.Instance;

		public Selections.Context<T> Selection => Selections.Context<T>.Instance;
	}
}