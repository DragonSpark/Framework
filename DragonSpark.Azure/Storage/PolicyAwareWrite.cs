using Azure.Storage.Blobs;
using DragonSpark.Diagnostics;

namespace DragonSpark.Azure.Storage;

sealed class PolicyAwareWrite : PolicyAware<WriteInput, BlobClient>, IWrite
{
	public PolicyAwareWrite(IWrite previous) : base(previous, DurableRequestPolicy<BlobClient>.Default) {}
}