using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class DetermineClientEntry : IStopAware<BlobClient, IStorageEntry?>
{
	public static DetermineClientEntry Default { get; } = new();

	DetermineClientEntry() : this(GetClientEntry.Default) {}

	readonly IStopAware<BlobBaseClient, DefaultStorageEntry> _previous;

	public DetermineClientEntry(IStopAware<BlobBaseClient, DefaultStorageEntry> previous) => _previous = previous;

	public async ValueTask<IStorageEntry?> Get(Stop<BlobClient> parameter)
		=> await parameter.Subject.ExistsAsync(parameter).Off() ? await _previous.Off(new(parameter, parameter)) : null;
}