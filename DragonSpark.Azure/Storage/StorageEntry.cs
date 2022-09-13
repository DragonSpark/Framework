using System.IO;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

public class StorageEntry : IStorageEntry
{
	readonly IStorageEntry _previous;

	public StorageEntry(IStorageEntry previous) : this(previous, previous.Properties) {}

	public StorageEntry(IStorageEntry previous, StorageEntryProperties properties)
	{
		_previous  = previous;
		Properties = properties;
	}

	public StorageEntryProperties Properties { get; }

	public ValueTask<Stream> Get() => _previous.Get();

	public ValueTask<Stream> Get(Stream parameter) => _previous.Get(parameter);
}