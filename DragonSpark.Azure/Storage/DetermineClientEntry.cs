using Azure.Storage.Blobs;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class DetermineClientEntry : ISelecting<BlobClient, DefaultStorageEntry?>
{
	public static DetermineClientEntry Default { get; } = new();

	DetermineClientEntry() : this(GetClientEntry.Default) {}

	readonly ISelecting<BlobClient, DefaultStorageEntry> _previous;

	public DetermineClientEntry(ISelecting<BlobClient, DefaultStorageEntry> previous) => _previous = previous;

	public async ValueTask<DefaultStorageEntry?> Get(BlobClient parameter)
		=> await parameter.ExistsAsync().ConfigureAwait(false) ? await _previous.Await(parameter) : default;
}