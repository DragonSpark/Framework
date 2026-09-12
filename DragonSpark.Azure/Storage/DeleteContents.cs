using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Storage;

sealed class DeleteContents : IDeleteContents
{
	readonly BlobContainerClient _client;

	public DeleteContents(BlobContainerClient client) => _client = client;

	public async ValueTask<bool> Get(Stop<string> parameter)
	{
		var (subject, stop) = parameter;
		var prefix = subject.EndsWith('/') ? subject : $"{subject}/";
		var result = true;

		await foreach (var page in _client.GetBlobsAsync(new() { Prefix = prefix }, cancellationToken: stop)
		                                  .AsPages()
		                                  .ConfigureAwait(false))
		{
			foreach (var item in page.Values)
			{
				if (item?.Name == null) continue;

				var response = await _client.GetBlobClient(item.Name)
				                            .DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: stop)
				                            .Off();
				if (!response.Value)
				{
					result = false; 
				}
			}
		}

		return result;
	}
}