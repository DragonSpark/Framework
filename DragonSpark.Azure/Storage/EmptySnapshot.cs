using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class EmptySnapshot : ISnapshotEntry
{
	public static EmptySnapshot Default { get; } = new();

	EmptySnapshot() {}

	public ValueTask Get(CancellationToken parameter) => ValueTask.CompletedTask;

	public ValueTask DisposeAsync()
	{
		return ValueTask.CompletedTask;
	}
}