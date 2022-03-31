using Azure.Storage.Blobs;
using DragonSpark.Compose;
using System.IO;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class StorageEntry : IStorageEntry
{
	readonly BlobClient _client;

	public StorageEntry(BlobClient client, StorageEntryProperties entry)
	{
		_client    = client;
		Properties = entry;
	}

	public StorageEntryProperties Properties { get; }

	public ValueTask<Stream> Get() => _client.OpenReadAsync().ToOperation();
}