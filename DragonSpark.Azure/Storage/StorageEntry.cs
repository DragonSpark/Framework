using DragonSpark.Model.Operations;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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

	public ValueTask<Stream> Get(CancellationToken parameter) => _previous.Get(parameter);

	public ValueTask<Stream> Get(Stop<Stream> parameter) => _previous.Get(parameter);

	public ValueTask Get(Stop<IDictionary<string, string?>> parameter) => _previous.Get(parameter);
}