using Azure.Storage.Blobs;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class GetClientEntry : ISelecting<BlobClient, StorageEntry>
{
	public static GetClientEntry Default { get; } = new();

	GetClientEntry() {}

	public async ValueTask<StorageEntry> Get(BlobClient parameter)
	{
		var response = await parameter.GetPropertiesAsync().ConfigureAwait(false);
		var value    = response.Value;
		var properties = new StorageEntryProperties(parameter.Uri, parameter.Name, value.ContentType,
		                                            (ulong)value.ContentLength, value.CreatedOn);
		return new(parameter, properties);
	}
}