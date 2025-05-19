using Azure.Storage.Blobs;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Results;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class DetermineClientEntry : ISelecting<BlobClient, IStorageEntry?>
{
	public static DetermineClientEntry Default { get; } = new();

	DetermineClientEntry() : this(GetClientEntry.Default, AmbientToken.Default) {}

	readonly ISelecting<BlobClient, DefaultStorageEntry> _previous;
	readonly IResult<CancellationToken>                  _stop;

	public DetermineClientEntry(ISelecting<BlobClient, DefaultStorageEntry> previous, IResult<CancellationToken> stop)
	{
		_previous  = previous;
		_stop = stop;
	}

	public async ValueTask<IStorageEntry?> Get(BlobClient parameter)
		=> await parameter.ExistsAsync(_stop.Get()).Off() ? await _previous.Off(parameter) : null;
}