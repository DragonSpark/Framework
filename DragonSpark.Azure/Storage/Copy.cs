using Azure.Storage.Blobs;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
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
		var operation = await client.StartCopyFromUriAsync(entry.Properties.Identity).Off();
		await operation.WaitForCompletionAsync().Off();
		var result = await _entry.Off(client);
		return result;
	}
}