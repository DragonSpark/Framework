using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Collections.Immutable;

namespace DragonSpark.Azure.Storage;

sealed class DeleteContents : IDeleteContents
{
	readonly BlobContainerClient _client;

	public DeleteContents(BlobContainerClient client) => _client = client;

	public async ValueTask<ImmutableArray<DeleteContentResult>> Get(Stop<string> parameter)
	{
		var (subject, stop) = parameter;
		var prefix  = subject.EndsWith('/') ? subject : $"{subject}/";
		var builder = ImmutableArray.CreateBuilder<DeleteContentResult>();
		await foreach (var page in _client.GetBlobsAsync(new() { Prefix = prefix }, cancellationToken: stop)
		                                  .AsPages()
		                                  .ConfigureAwait(false))
		{
			foreach (var item in page.Values.Where(x => x.Name is not null).Select(x => x.Name.Verify()))
			{
				var response = await _client.GetBlobClient(item)
				                            .DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots,
				                                                 cancellationToken: stop)
				                            .Off();
				builder.Add(new(item, response.Value));
			}
		}

		var result = builder.ToImmutable();
		return result;
	}
}