using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public sealed class SelectTask<T> : Select<ValueTask<T>, Task<T>>
{
	public static SelectTask<T> Default { get; } = new();

	SelectTask() : base(x => x.AsTask()) {}
}

public sealed class SelectTask : Select<ValueTask, Task>
{
	public static SelectTask Default { get; } = new();

	SelectTask() : base(x => x.AsTask()) {}
}