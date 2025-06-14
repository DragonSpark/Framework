using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class Copy : ICopy
{
	readonly BlobContainerClient                       _client;
	readonly IStopAware<BlobBaseClient, IStorageEntry> _entry;

	public Copy(BlobContainerClient client) : this(client, CreateClientEntry.Default) {}

	public Copy(BlobContainerClient client, IStopAware<BlobBaseClient, IStorageEntry> entry)
	{
		_client = client;
		_entry  = entry;
	}

	public async ValueTask<IStorageEntry> Get(Stop<DestinationInput> parameter)
	{
		var ((entry, destination), stop) = parameter;
		var client    = _client.GetBlobClient(destination);
		var operation = await client.StartCopyFromUriAsync(entry.Properties.Identity, cancellationToken: stop).Off();
		await operation.WaitForCompletionAsync(stop).Off();
		var result = await _entry.Off(new(client, stop));
		return result;
	}
}