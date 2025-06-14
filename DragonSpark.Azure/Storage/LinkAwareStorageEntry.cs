using Azure.Storage.Blobs.Specialized;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class LinkAwareStorageEntry : INewStorageEntry
{
	public static LinkAwareStorageEntry Default { get; } = new();

	LinkAwareStorageEntry() : this(NewStorageEntry.Default) {}

	readonly INewStorageEntry _previous;

	public LinkAwareStorageEntry(INewStorageEntry previous) => _previous = previous;

	public async ValueTask<IStorageEntry> Get(Stop<EntryInput> parameter)
	{
		var ((client, properties), stop) = parameter;
		var path = properties.Metadata.Count > 0 ? new LinkPathVariable(properties.Metadata).Get() : null;
		if (path is not null)
		{
			var linked   = client.GetParentBlobContainerClient().GetBlobClient(path);
			var response = await linked.GetPropertiesAsync(cancellationToken: stop).Off();
			var value    = response.Value;
			return await _previous.Off(new(new(linked, value), stop));
		}

		return await _previous.Off(parameter);
	}
}