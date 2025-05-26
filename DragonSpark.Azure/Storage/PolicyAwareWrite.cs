using Azure.Storage.Blobs;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Storage;

sealed class PolicyAwareWrite : PolicyAwareSelecting<Stop<WriteInput>, BlobClient>, IWrite
{
	public PolicyAwareWrite(IWrite previous) : base(previous, DurableRequestPolicy<BlobClient>.Default.Get()) {}
}