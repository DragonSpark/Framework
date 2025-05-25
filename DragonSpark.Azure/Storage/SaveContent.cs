using Azure.Storage.Blobs.Specialized;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class SaveContent : ISaveContent
{
	readonly IWrite                                          _write;
	readonly IStopAware<BlobBaseClient, DefaultStorageEntry> _entry;

	public SaveContent(IWrite write) : this(write, GetClientEntry.Default) {}

	public SaveContent(IWrite write, IStopAware<BlobBaseClient, DefaultStorageEntry> entry)
	{
		_write = write;
		_entry = entry;
	}

	public async ValueTask<IStorageEntry> Get(Stop<WriteInput> parameter)
	{
		var client = await _write.Off(parameter);
		var result = await _entry.Off(new(client, parameter));
		return result;
	}
}