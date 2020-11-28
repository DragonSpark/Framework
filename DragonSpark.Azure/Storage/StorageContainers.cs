using Azure.Storage.Blobs;

namespace DragonSpark.Azure.Storage
{
	sealed class StorageContainers : IStorageContainers
	{
		readonly BlobServiceClient _client;

		public StorageContainers(BlobServiceClient client) => _client = client;

		public BlobContainerClient Get(string parameter) => _client.GetBlobContainerClient(parameter);
	}
}