using Azure.Storage.Blobs;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Storage
{
	sealed class ValidatedStorageContainers : Select<string, BlobContainerClient>, IStorageContainers
	{
		public ValidatedStorageContainers(IStorageContainers previous)
			: base(Start.A.Selection<string>().By.Calling(x => x.ToLowerInvariant()).Select(previous)) {}
	}
}