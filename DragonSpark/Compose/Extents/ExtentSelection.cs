using DragonSpark.Compose.Extents.Commands;
using DragonSpark.Compose.Extents.Conditions;
using DragonSpark.Compose.Extents.Results;
using DragonSpark.Compose.Extents.Selections;

namespace DragonSpark.Compose.Extents;

public sealed class ExtentSelection<T>
{
	public static ExtentSelection<T> Default { get; } = new();

	ExtentSelection() {}

	public ResultComposer<T> Result => ResultComposer<T>.Instance;

	public ConditionContext<T> Condition => ConditionContext<T>.Instance;

	public CommandComposer<T> Command => CommandComposer<T>.Instance;

	public SelectionContext<T> Selection => SelectionContext<T>.Instance;
}