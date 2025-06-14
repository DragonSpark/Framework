using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class GetClientEntry : IStopAware<BlobClient, IStorageEntry?>
{
	public static GetClientEntry Default { get; } = new();

	GetClientEntry() : this(new CreateClientEntry(LinkAwareStorageEntry.Default)) {}

	readonly IStopAware<BlobBaseClient, IStorageEntry> _previous;

	public GetClientEntry(IStopAware<BlobBaseClient, IStorageEntry> previous) => _previous = previous;

	public async ValueTask<IStorageEntry?> Get(Stop<BlobClient> parameter)
	{
		var (subject, stop) = parameter;
		return await subject.ExistsAsync(stop).Off() ? await _previous.Off(new(subject, stop)) : null;
	}
}