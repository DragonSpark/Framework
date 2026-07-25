using Azure.Storage.Blobs;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Storage;

sealed class Snapshot : ISnapshot
{
	readonly BlobContainerClient _client;
	readonly ISnapshotEntry      _default;

	public Snapshot(BlobContainerClient client) : this(client, EmptySnapshot.Default) {}

	public Snapshot(BlobContainerClient client, ISnapshotEntry @default)
	{
		_client  = client;
		_default = @default;
	}

	public async ValueTask<ISnapshotEntry> Get(Stop<string> parameter)
	{
		var (subject, stop) = parameter;
		var client = _client.GetBlobClient(subject);
		return await client.ExistsAsync(stop).Off()
			       ? new SnapshotEntry(client, await client.CreateSnapshotAsync(cancellationToken: stop).Off())
			       : _default;
	}
}