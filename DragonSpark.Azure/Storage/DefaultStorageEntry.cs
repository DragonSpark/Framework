using Azure.Storage.Blobs.Specialized;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.IO;
using System.Threading;
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

	public ValueTask<Stream> Get(CancellationToken parameter)
		=> _client.OpenReadAsync(cancellationToken: parameter).ToOperation();

	public async ValueTask<Stream> Get(Stop<Stream> parameter)
	{
		await _client.DownloadToAsync(parameter, parameter).Off();
		return parameter;
	}
}