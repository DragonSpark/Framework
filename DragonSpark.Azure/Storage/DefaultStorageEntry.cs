using Azure.Storage.Blobs.Specialized;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class DefaultStorageEntry : IStorageEntry
{
	readonly BlobBaseClient _client;

	public DefaultStorageEntry(BlobBaseClient client, StorageEntryProperties properties)
	{
		_client    = client;
		Properties = properties;
	}

	public StorageEntryProperties Properties { get; }

	public ValueTask<Stream> Get(CancellationToken parameter)
		=> _client.OpenReadAsync(cancellationToken: parameter).ToOperation();

	public async ValueTask<Stream> Get(Stop<Stream> parameter)
	{
		await _client.DownloadToAsync(parameter, parameter).Off();
		return parameter;
	}

	public async ValueTask Get(Stop<IDictionary<string, string?>> parameter)
		=> await _client.SetMetadataAsync(parameter.Subject, cancellationToken: parameter).Off();
}