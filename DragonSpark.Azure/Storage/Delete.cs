using Azure.Storage.Blobs;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class Delete : IDelete
{
	readonly BlobContainerClient _client;

	public Delete(BlobContainerClient client) => _client = client;

	public async ValueTask<bool> Get(Stop<string> parameter)
	{
		var client   = _client.GetBlobClient(parameter);
		var response = await client.DeleteIfExistsAsync(cancellationToken: parameter).Off();
		return response.Value;
	}
}