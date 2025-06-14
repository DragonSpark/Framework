using Azure.Storage.Blobs;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class Entry : IEntry
{
	readonly BlobContainerClient                    _client;
	readonly IStopAware<BlobClient, IStorageEntry?> _entry;

	public Entry(BlobContainerClient client) : this(client, GetClientEntry.Default) {}

	public Entry(BlobContainerClient client, IStopAware<BlobClient, IStorageEntry?> entry)
	{
		_client = client;
		_entry  = entry;
	}

	public ValueTask<IStorageEntry?> Get(Stop<string> parameter)
	{
		var (subject, stop) = parameter;
		var client = _client.GetBlobClient(subject);
		var result = _entry.Get(client.Stop(stop));
		return result;
	}
}