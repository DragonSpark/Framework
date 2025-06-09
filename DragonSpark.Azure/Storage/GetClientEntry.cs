using Azure.Storage.Blobs.Specialized;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Text;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class GetClientEntry : IStopAware<BlobBaseClient, DefaultStorageEntry>
{
	public static GetClientEntry Default { get; } = new();

	GetClientEntry() : this(EntryName.Default) {}

	readonly IFormatter<EntryNameInput> _name;

	public GetClientEntry(IFormatter<EntryNameInput> name) => _name = name;

	public async ValueTask<DefaultStorageEntry> Get(Stop<BlobBaseClient> parameter)
	{
		var (subject, stop) = parameter;
		var response = await subject.GetPropertiesAsync(cancellationToken: stop).Off();
		var value    = response.Value;
		var name     = _name.Get(new(subject, value));
		return new(parameter, new(subject.Uri, subject.Name, name, value.ContentType, (ulong)value.ContentLength,
		                          value.CreatedOn, value.LastModified, value.ETag, value.Metadata));
	}
}