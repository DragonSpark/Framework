using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public sealed class EmptyAllocated : Allocated
{
	public static EmptyAllocated Default { get; } = new();

	EmptyAllocated() : base(() => Task.CompletedTask) {}
}