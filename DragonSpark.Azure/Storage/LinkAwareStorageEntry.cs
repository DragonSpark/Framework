using Azure.Storage.Blobs.Specialized;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class LinkAwareStorageEntry : INewStorageEntry
{
	public static LinkAwareStorageEntry Default { get; } = new();

	LinkAwareStorageEntry() : this(NewStorageEntry.Default, LinkPathProperty.Default) {}

	readonly INewStorageEntry      _previous;
	readonly IEntryProperty _entry;

	public LinkAwareStorageEntry(INewStorageEntry previous, IEntryProperty entry)
	{
		_previous = previous;
		_entry    = entry;
	}

	public async ValueTask<IStorageEntry> Get(Stop<EntryInput> parameter)
	{
		var ((client, properties), stop) = parameter;
		var path = properties.Metadata.Count > 0 ? _entry.Get(properties.Metadata) : null;
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