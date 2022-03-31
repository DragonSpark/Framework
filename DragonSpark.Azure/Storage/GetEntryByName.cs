using Azure.Storage.Blobs;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class GetEntryByName : ISelecting<string, StorageEntry?>
{
	readonly BlobContainerClient                   _client;
	readonly ISelecting<BlobClient, StorageEntry?> _entry;

	public GetEntryByName(BlobContainerClient client) : this(client, DetermineClientEntry.Default) {}

	public GetEntryByName(BlobContainerClient client, ISelecting<BlobClient, StorageEntry?> entry)
	{
		_client = client;
		_entry  = entry;
	}

	public ValueTask<StorageEntry?> Get(string parameter)
	{
		var client = _client.GetBlobClient(parameter);
		var result = _entry.Get(client);
		return result;
	}
}