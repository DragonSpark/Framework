using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations;

public sealed class SelectOperation : Select<Task, ValueTask>
{
	public static SelectOperation Default { get; } = new();

	SelectOperation() : base(x => new (x)) {}
}