using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class SaveContent : ISaveContent
{
	readonly BlobContainerClient           _client;
	readonly ISelecting<BlobClient, StorageEntry> _entry;

	public SaveContent(BlobContainerClient client) : this(client, GetClientEntry.Default) {}

	public SaveContent(BlobContainerClient client, ISelecting<BlobClient, StorageEntry> entry)
	{
		_client = client;
		_entry  = entry;
	}

	public async ValueTask<IStorageEntry> Get(NewStorageEntryInput parameter)
	{
		var (name, contentType, _, _, write) = parameter;
		var             client = _client.GetBlobClient(name);
		var             header = new BlobHttpHeaders { ContentType = contentType };
		await using var stream = await client.OpenWriteAsync(false).ConfigureAwait(false);
		await write(stream).ConfigureAwait(false);
		await client.UploadAsync(stream, header).ConfigureAwait(false);
		var result = await _entry.Await(client);
		return result;
	}
}

// TODO:

sealed class GetEntryByName : ISelecting<string, StorageEntry?>
{
	readonly BlobContainerClient            _client;
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

sealed class GetClientEntry : ISelecting<BlobClient, StorageEntry>
{
	public static GetClientEntry Default { get; } = new();

	GetClientEntry() {}

	public async ValueTask<StorageEntry> Get(BlobClient parameter)
	{
		var response = await parameter.GetPropertiesAsync().ConfigureAwait(false);
		var value    = response.Value;
		return new(parameter,
		           new(parameter.Uri, parameter.Name, value.ContentType, (ulong)value.ContentLength, value.CreatedOn));
	}
}

sealed class DetermineClientEntry : ISelecting<BlobClient, StorageEntry?>
{
	public static DetermineClientEntry Default { get; } = new();

	DetermineClientEntry() : this(GetClientEntry.Default) {}

	readonly ISelecting<BlobClient, StorageEntry> _previous;

	public DetermineClientEntry(ISelecting<BlobClient, StorageEntry> previous) => _previous = previous;

	public async ValueTask<StorageEntry?> Get(BlobClient parameter)
		=> await parameter.ExistsAsync().ConfigureAwait(false) ? await _previous.Await(parameter) : default;
}

public interface IStorageEntry : IResulting<Stream>
{
	StorageEntryProperties Properties { get; }
}

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

public sealed record StorageEntryProperties(Uri Identity, string Name, string ContentType, ulong Size, DateTimeOffset Created);