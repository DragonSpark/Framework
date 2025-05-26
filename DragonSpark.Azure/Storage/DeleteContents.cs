using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using NetFabric.Hyperlinq;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class DeleteContents : IDeleteContents
{
	readonly BlobContainerClient _client;

	public DeleteContents(BlobContainerClient client) => _client = client;

	public async ValueTask<bool> Get(Stop<string> parameter)
	{
		var (subject, stop) = parameter;
		await foreach (var page in _client.GetBlobsAsync(prefix: subject, cancellationToken: stop)
		                                  .AsPages()
		                                  .ConfigureAwait(false))
		{
			foreach (var item in page.Values.AsValueEnumerable())
			{
				var response = await _client.GetBlobClient(item!.Name)
				                            .DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots,
				                                                 cancellationToken: stop)
				                            .Off();
				if (!response.Value)
				{
					return false;
				}
			}
		}

		return true;
	}
}