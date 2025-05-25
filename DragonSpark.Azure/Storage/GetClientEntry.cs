using Azure.Storage.Blobs.Specialized;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class GetClientEntry : IStopAware<BlobBaseClient, DefaultStorageEntry>
{
	public static GetClientEntry Default { get; } = new();

	GetClientEntry() {}

	public async ValueTask<DefaultStorageEntry> Get(Stop<BlobBaseClient> parameter)
	{
		var (subject, stop) = parameter;
		var response = await subject.GetPropertiesAsync(cancellationToken: stop).Off();
		var value    = response.Value;
		return new(parameter,
		           new(subject.Uri, subject.Name, value.ContentType,
		               (ulong)value.ContentLength, value.CreatedOn, value.LastModified,
		               value.ETag));
	}
}