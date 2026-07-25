using DragonSpark.Model.Operations;
using DragonSpark.Text;

namespace DragonSpark.Azure.Storage;

sealed class LoadStorageEntry : ILoadStorageEntry
{
	public static LoadStorageEntry Default { get; } = new();

	LoadStorageEntry() : this(EntryName.Default) {}

	readonly IFormatter<EntryInput> _name;

	public LoadStorageEntry(IFormatter<EntryInput> name) => _name = name;

	public ValueTask<IStorageEntry> Get(Stop<EntryInput> parameter)
	{
		var ((client, model), _) = parameter;
		var name = _name.Get(parameter);
		var properties = new StorageEntryProperties(client.Uri, client.Name, name, model.ContentType,
		                                            (ulong)model.ContentLength, model.CreatedOn, model.LastModified,
		                                            model.ETag, model.Metadata);
		var entry = new DefaultStorageEntry(client, properties);
		return new(entry);
	}
}