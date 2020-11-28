using Azure.Storage.Blobs;
using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Storage
{
	public sealed class StorageClients : ISelect<string, BlobServiceClient>
	{
		public static StorageClients Default { get; } = new StorageClients();

		StorageClients() {}

		public BlobServiceClient Get(string parameter) => new BlobServiceClient(parameter);
	}
}