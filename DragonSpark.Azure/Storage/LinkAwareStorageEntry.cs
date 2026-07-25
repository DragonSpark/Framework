using Azure.Storage.Blobs.Specialized;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Storage;

sealed class LinkAwareStorageEntry : ILoadStorageEntry
{
	public static LinkAwareStorageEntry Default { get; } = new();

	LinkAwareStorageEntry() : this(LoadStorageEntry.Default, LinkPathProperty.Default) {}

	readonly ILoadStorageEntry _previous;
	readonly IEntryProperty    _entry;

	public LinkAwareStorageEntry(ILoadStorageEntry previous, IEntryProperty entry)
	{
		_previous = previous;
		_entry    = entry;
	}

	public async ValueTask<IStorageEntry> Get(Stop<EntryInput> parameter)
	{
		var ((client, properties), stop) = parameter;

		var input = new EntryInput(client, properties);
		while (input.Properties.Metadata.Count > 0 && _entry.Get(input.Properties.Metadata) is {} path)
		{
			var next     = input.Client.GetParentBlobContainerClient().GetBlobClient(path);
			var response = await next.GetPropertiesAsync(cancellationToken: stop).Off();
			input = new(next, response.Value);
		}

		return await _previous.Off(new(input, stop));
	}
}