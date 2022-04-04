using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using DragonSpark.Model.Operations;
using System.IO;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class Write : IWrite
{
	readonly BlobContainerClient _client;

	public Write(BlobContainerClient client) => _client = client;

	public async ValueTask<BlobClient> Get(NewStorageEntryInput parameter)
	{
		var (name, contentType, _, _, write) = parameter;
		var             result  = _client.GetBlobClient(name);
		var             header  = new BlobHttpHeaders { ContentType      = contentType };
		var             options = new BlobOpenWriteOptions { HttpHeaders = header };
		await using var stream  = await result.OpenWriteAsync(true, options).ConfigureAwait(false);
		await write(stream).ConfigureAwait(false);
		await stream.FlushAsync().ConfigureAwait(false);
		return result;
	}
}

// TODO:

public interface IAppend : ISelecting<AppendInput, BlobBaseClient> {}

sealed class Append : IAppend
{
	readonly BlobContainerClient _client;

	public Append(BlobContainerClient client) => _client = client;

	public async ValueTask<BlobBaseClient> Get(AppendInput parameter)
	{
		var (name, contentType, content) = parameter;
		var result = _client.GetAppendBlobClient(name);
		await result.CreateIfNotExistsAsync(new BlobHttpHeaders { ContentType = contentType }).ConfigureAwait(false);
		await result.AppendBlockAsync(content).ConfigureAwait(false);
		return result;
	}
}

public readonly record struct AppendInput(string Name, string Type, Stream Stream);