using Azure.Storage.Blobs.Specialized;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class CreateClientEntry : IStopAware<BlobBaseClient, IStorageEntry>
{
	public static CreateClientEntry Default { get; } = new();

	CreateClientEntry() : this(NewStorageEntry.Default) {}

	readonly INewStorageEntry _entry;

	public CreateClientEntry(INewStorageEntry entry) => _entry = entry;

	public async ValueTask<IStorageEntry> Get(Stop<BlobBaseClient> parameter)
	{
		var (subject, stop) = parameter;
		var response = await subject.GetPropertiesAsync(cancellationToken: stop).Off();
		var value    = response.Value;
		return await _entry.Off(new(new(subject, value), stop));
	}
}