using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public sealed class EmptyOperation<T> : Operation<T>
{
	public static EmptyOperation<T> Default { get; } = new();

	EmptyOperation() : base(_ => ValueTask.CompletedTask) {}
}

public sealed class EmptyOperation : Operation
{
	public static EmptyOperation Default { get; } = new();

	EmptyOperation() : base(() => ValueTask.CompletedTask) {}
}