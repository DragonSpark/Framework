using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class Write : IWrite
{
	readonly BlobContainerClient _client;

	public Write(BlobContainerClient client) => _client = client;

	public async ValueTask<BlobClient> Get(WriteInput parameter)
	{
		var (name, contentType, write) = parameter;
		var             result  = _client.GetBlobClient(name);
		var             header  = new BlobHttpHeaders { ContentType      = contentType };
		var             options = new BlobOpenWriteOptions { HttpHeaders = header };
		await using var stream  = await result.OpenWriteAsync(true, options).ConfigureAwait(false);
		await write(stream).ConfigureAwait(false);
		await stream.FlushAsync().ConfigureAwait(false);
		return result;
	}
}