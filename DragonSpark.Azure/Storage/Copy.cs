using Azure.Storage.Blobs;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class Copy : ICopy
{
	readonly BlobContainerClient                  _client;
	readonly ISelecting<BlobClient, DefaultStorageEntry> _entry;

	public Copy(BlobContainerClient client) : this(client, GetClientEntry.Default) {}

	public Copy(BlobContainerClient client, ISelecting<BlobClient, DefaultStorageEntry> entry)
	{
		_client = client;
		_entry  = entry;
	}

	public async ValueTask<IStorageEntry> Get(DestinationInput parameter)
	{
		var (entry, destination) = parameter;
		var client    = _client.GetBlobClient(destination);
		var operation = await client.StartCopyFromUriAsync(entry.Properties.Identity).ConfigureAwait(false);
		await operation.WaitForCompletionAsync().ConfigureAwait(false);
		var result = await _entry.Await(client);
		return result;
	}
}