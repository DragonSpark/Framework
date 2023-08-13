using Medallion.Threading;
using Medallion.Threading.Azure;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

public class DistributedLock : IDistributedLock
{
	readonly AzureBlobLeaseDistributedLock _lock;

	public DistributedLock(IContainer container, string name) : this(new(container.Get(), name)) {}

	public DistributedLock(AzureBlobLeaseDistributedLock @lock) => _lock  = @lock;

	public async ValueTask<IDistributedSynchronizationHandle> Get() => await _lock.AcquireAsync().ConfigureAwait(false);
}