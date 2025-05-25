using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class Append : IAppend
{
	readonly BlobContainerClient _client;

	public Append(BlobContainerClient client) => _client = client;

	public async ValueTask<BlobBaseClient> Get(Stop<AppendInput> parameter)
	{
		var ((name, contentType, content), stop) = parameter;
		var result = _client.GetAppendBlobClient(name);
		await result.CreateIfNotExistsAsync(new BlobHttpHeaders { ContentType = contentType }, cancellationToken: stop)
		            .Off();
		await result.AppendBlockAsync(content, cancellationToken: stop).Off();
		return result;
	}
}