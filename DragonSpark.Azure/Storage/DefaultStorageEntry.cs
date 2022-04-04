using Azure.Storage.Blobs.Specialized;
using DragonSpark.Compose;
using System.IO;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class DefaultStorageEntry : IStorageEntry
{
	readonly BlobBaseClient _client;

	public DefaultStorageEntry(BlobBaseClient client, StorageEntryProperties entry)
	{
		_client    = client;
		Properties = entry;
	}

	public StorageEntryProperties Properties { get; }

	public ValueTask<Stream> Get() => _client.OpenReadAsync().ToOperation();
}