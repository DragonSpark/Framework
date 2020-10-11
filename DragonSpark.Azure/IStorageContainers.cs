using Azure.Storage.Blobs;
using DragonSpark.Model.Selection;

namespace DragonSpark.Azure
{
	public interface IStorageContainers : ISelect<string, BlobContainerClient> {}
}