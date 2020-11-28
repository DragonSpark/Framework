using Azure.Storage.Blobs;
using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Storage
{
	public interface IStorageContainers : ISelect<string, BlobContainerClient> {}
}