using Azure.Storage.Blobs;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class SaveContent : ISaveContent
{
	readonly IWrite                               _write;
	readonly ISelecting<BlobClient, StorageEntry> _entry;

	public SaveContent(IWrite write) : this(write, GetClientEntry.Default) {}

	public SaveContent(IWrite write, ISelecting<BlobClient, StorageEntry> entry)
	{
		_write = write;
		_entry = entry;
	}

	public async ValueTask<IStorageEntry> Get(NewStorageEntryInput parameter)
	{
		var client = await _write.Await(parameter);
		var result = await _entry.Await(client);
		return result;
	}
}