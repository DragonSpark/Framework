using Azure.Storage.Blobs;
using DragonSpark.Model.Operations.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class Entry : IEntry
{
	readonly BlobContainerClient                    _client;
	readonly ISelecting<BlobClient, IStorageEntry?> _entry;

	public Entry(BlobContainerClient client) : this(client, DetermineClientEntry.Default) {}

	public Entry(BlobContainerClient client, ISelecting<BlobClient, IStorageEntry?> entry)
	{
		_client = client;
		_entry  = entry;
	}

	public ValueTask<IStorageEntry?> Get(string parameter)
	{
		var client = _client.GetBlobClient(parameter);
		var result = _entry.Get(client);
		return result;
	}
}