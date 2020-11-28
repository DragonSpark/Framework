using Azure.Storage.Blobs;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Storage
{
	public interface IContainer : IResult<BlobContainerClient> {}
}