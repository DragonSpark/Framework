using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Azure.Storage;

sealed class SnapshotsEntry : ISnapshotEntry
{
	readonly Leasing<ISnapshotEntry> _entries;

	public SnapshotsEntry(Leasing<ISnapshotEntry> entries) => _entries = entries;

	public async ValueTask Get(CancellationToken parameter)
	{
		foreach (var entry in _entries)
		{
			await entry.Off(parameter);
		}
	}

	public async ValueTask DisposeAsync()
	{
		foreach (var entry in _entries)
		{
			await entry.DisposeAsync().Off();
		}
		_entries.Dispose();
	}
}