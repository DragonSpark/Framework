using DragonSpark.Compose.Results;

namespace DragonSpark.Compose.Extents
{
	public sealed class ExtentSelection<T>
	{
		public static ExtentSelection<T> Default { get; } = new ExtentSelection<T>();

		ExtentSelection() {}

		public Context<T> Result => Context<T>.Instance;

		public Conditions.Context<T> Condition => Conditions.Context<T>.Instance;

		public Commands.Context<T> Command => Commands.Context<T>.Instance;

		public Selections.Context<T> Selection => Selections.Context<T>.Instance;
	}
}