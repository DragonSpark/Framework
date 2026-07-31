using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime;

namespace DragonSpark.Azure.Storage;

sealed class DefaultStorageEntry : IStorageEntry
{
	readonly BlobBaseClient _client;
	readonly ITime          _time;

	public DefaultStorageEntry(BlobBaseClient client, StorageEntryProperties properties)
		: this(client, properties, Time.Default) {}

	public DefaultStorageEntry(BlobBaseClient client, StorageEntryProperties properties, ITime time)
	{
		_client    = client;
		_time      = time;
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

	public async ValueTask<Uri> Get(Stop<RelayInput> parameter)
	{
		var ((name, contentType, start, access, content), stop) = parameter;
		var time = _time.Get();
		var builder = new BlobSasBuilder
		{
			BlobContainerName  = _client.BlobContainerName,
			BlobName           = _client.Name,
			Resource           = "b",
			StartsOn           = time.Add(start),
			ContentDisposition = name is not null ? @$"attachment; filename=""{name}""" : "inline",
			ContentType        = contentType,
			CacheControl       = $"private, max-age={content.TotalSeconds:0}",
			ExpiresOn          = time.Add(access),
			Protocol           = SasProtocol.Https
		};
		builder.SetPermissions(BlobSasPermissions.Read);

		var key = await _client.GetParentBlobContainerClient()
		                       .GetParentBlobServiceClient()
		                       .GetUserDelegationKeyAsync(new(time.Add(access)) { StartsOn = time.Add(start) }, stop)
		                       .Off();

		var parameters = builder.ToSasQueryParameters(key.Value, _client.AccountName);
		var address    = new UriBuilder(_client.Uri) { Query = parameters.ToString() };
		return address.Uri;
	}
}