using Azure.Storage.Blobs;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class DetermineClientEntry : ISelecting<BlobClient, StorageEntry?>
{
	public static DetermineClientEntry Default { get; } = new();

	DetermineClientEntry() : this(GetClientEntry.Default) {}

	readonly ISelecting<BlobClient, StorageEntry> _previous;

	public DetermineClientEntry(ISelecting<BlobClient, StorageEntry> previous) => _previous = previous;

	public async ValueTask<StorageEntry?> Get(BlobClient parameter)
		=> await parameter.ExistsAsync().ConfigureAwait(false) ? await _previous.Await(parameter) : default;
}