using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class Write : IWrite
{
	readonly BlobContainerClient _client;

	public Write(BlobContainerClient client) => _client = client;

	public async ValueTask<BlobClient> Get(Stop<WriteInput> parameter)
	{
		var ((name, contentType, write), stop) = parameter;
		var             result  = _client.GetBlobClient(name);
		var             header  = new BlobHttpHeaders { ContentType      = contentType };
		var             options = new BlobOpenWriteOptions { HttpHeaders = header };
		await using var stream  = await result.OpenWriteAsync(true, options, stop).Off();
		await write(stream, stop).Off();
		await stream.FlushAsync(stop).Off();
		return result;
	}
}