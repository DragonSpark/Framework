using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DragonSpark.Compose;

namespace DragonSpark.Azure.Storage;

public class SnapshotEntry : ISnapshotEntry
{
	readonly BlobClient _entry;
	readonly BlobClient _snapshot;

	public SnapshotEntry(BlobClient entry, BlobSnapshotInfo information)
		: this(entry, entry.WithSnapshot(information.Snapshot)) {}

	public SnapshotEntry(BlobClient entry, BlobClient snapshot)
	{
		_entry    = entry;
		_snapshot = snapshot;
	}

	public async ValueTask Get(CancellationToken parameter)
	{
		var start = await _entry.StartCopyFromUriAsync(_snapshot.Uri, cancellationToken: parameter).Off();
		await start.WaitForCompletionAsync(parameter).Off();
	}

	public async ValueTask DisposeAsync()
	{
		await _snapshot.DeleteAsync(cancellationToken: CancellationToken.None).Off();
	}
}