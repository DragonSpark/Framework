using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DragonSpark.Compose;
using NetFabric.Hyperlinq;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class DeleteContents : IDeleteContents
{
	readonly BlobContainerClient _client;

	public DeleteContents(BlobContainerClient client) => _client = client;

	public async ValueTask<bool> Get(string parameter)
	{
		// Enumerate the blobs returned for each page.
		await foreach (var page in _client.GetBlobsAsync(prefix: parameter).AsPages().ConfigureAwait(false))
		{
			foreach (var item in page.Values.AsValueEnumerable())
			{
				var response = await _client.GetBlobClient(item!.Name)
				                            .DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots)
				                            .Await();
				if (!response.Value)
				{
					return false;
				}
			}
		}

		return true;
	}
}