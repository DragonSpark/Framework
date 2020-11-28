using Azure.Storage.Blobs;
using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Storage
{
	public class Container : Result<BlobContainerClient>, IContainer
	{
		public Container(IStorageContainers containers, string name) : base(containers.Then().Bind(name)) {}
	}
}