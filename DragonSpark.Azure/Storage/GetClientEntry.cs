using Azure.Storage.Blobs.Specialized;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Results;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class GetClientEntry : ISelecting<BlobBaseClient, DefaultStorageEntry>
{
	public static GetClientEntry Default { get; } = new();

	GetClientEntry() : this(AmbientToken.Default) {}

	readonly IResult<CancellationToken> _stop;

	public GetClientEntry(IResult<CancellationToken> stop) => _stop = stop;

	public async ValueTask<DefaultStorageEntry> Get(BlobBaseClient parameter)
	{
		var response = await parameter.GetPropertiesAsync(cancellationToken: _stop.Get()).Off();
		var value    = response.Value;
		return new(parameter,
		           new(parameter.Uri, parameter.Name, value.ContentType,
		               (ulong)value.ContentLength, value.CreatedOn, value.LastModified,
		               value.ETag));
	}
}