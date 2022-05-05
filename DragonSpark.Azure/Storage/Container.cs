using Azure.Storage.Blobs;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Storage;

public class Container : FixedSelection<string, BlobContainerClient>, IContainer
{
	protected Container(IStorageContainers containers, string name) : base(containers, name) {}
}