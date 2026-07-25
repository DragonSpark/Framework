using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Storage;

public class EntryProperty : IEntryProperty
{
	readonly string      _name;
	readonly IEntryTable _table;

	protected EntryProperty(string name) : this(name, EntryTables.Default) {}

	protected EntryProperty(string name, IEntryTable table)
	{
		_name  = name;
		_table = table;
	}

	public string? Get(IDictionary<string, string?> parameter) => _table.Get(parameter).Get(_name);

	public void Execute(Pair<IDictionary<string, string?>, string?> parameter)
	{
		var (key, value) = parameter;
		_table.Get(key).Assign(_name, value);
	}

	public string? Get(IStorageEntry parameter) => Get(parameter.Properties.Metadata);

	public ValueTask Get(Stop<Pair<IStorageEntry, string>> parameter)
	{
		var ((key, value), stop) = parameter;
		var metadata = key.Properties.Metadata;
		Execute((metadata, value));
		return key.Get(metadata.Stop(stop));
	}
}