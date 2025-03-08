using Azure.Storage.Blobs;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class DetermineClientEntry : ISelecting<BlobClient, IStorageEntry?>
{
	public static DetermineClientEntry Default { get; } = new();

	DetermineClientEntry() : this(GetClientEntry.Default) {}

	readonly ISelecting<BlobClient, DefaultStorageEntry> _previous;

	public DetermineClientEntry(ISelecting<BlobClient, DefaultStorageEntry> previous) => _previous = previous;

	public async ValueTask<IStorageEntry?> Get(BlobClient parameter)
		=> await parameter.ExistsAsync().Off() ? await _previous.Off(parameter) : null;
}