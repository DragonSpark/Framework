using Azure.Storage.Blobs;
using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Storage
{
	sealed class ConfiguredStorageClient : Result<BlobServiceClient>
	{
		public ConfiguredStorageClient(AzureStorageConfiguration configuration)
			: base(StorageClients.Default.Then().Bind(configuration.Connection)) {}
	}

	
}