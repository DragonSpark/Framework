using Azure.Storage.Blobs;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class Delete : IDelete
{
	readonly BlobContainerClient _client;

	public Delete(BlobContainerClient client) => _client = client;

	public async ValueTask<bool> Get(string parameter)
	{
		var client   = _client.GetBlobClient(parameter);
		var response = await client.DeleteIfExistsAsync().ConfigureAwait(false);
		return response.Value;
	}
}