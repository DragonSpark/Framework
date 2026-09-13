using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Azure.Storage;

class ClientEntryBase : IStopAware<BlobClient, IStorageEntry?>
{
	readonly IStopAware<BlobBaseClient, IStorageEntry?> _previous;

	protected ClientEntryBase(ILoadStorageEntry load) : this(new LoadClientEntry(load)) {}

	protected ClientEntryBase(IStopAware<BlobBaseClient, IStorageEntry?> previous) => _previous = previous;

	public ValueTask<IStorageEntry?> Get(Stop<BlobClient> parameter)
	{
		var (subject, stop) = parameter;
		return _previous.Get(new(subject, stop));
	}
}