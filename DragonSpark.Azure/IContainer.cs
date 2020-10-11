using Azure.Storage.Blobs;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure
{
	public interface IContainer : IResult<BlobContainerClient> {}
}