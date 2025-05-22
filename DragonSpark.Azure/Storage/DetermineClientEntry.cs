using Azure.Storage.Blobs;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Selection.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class DetermineClientEntry : IStopAware<BlobClient, IStorageEntry?>
{
	public static DetermineClientEntry Default { get; } = new();

	DetermineClientEntry() : this(GetClientEntry.Default) {}

	readonly ISelecting<BlobClient, DefaultStorageEntry> _previous;
	
	public DetermineClientEntry(ISelecting<BlobClient, DefaultStorageEntry> previous) => _previous = previous;

	public async ValueTask<IStorageEntry?> Get(Stop<BlobClient> parameter)
		=> await parameter.Subject.ExistsAsync(parameter).Off() ? await _previous.Off(parameter) : null;
}