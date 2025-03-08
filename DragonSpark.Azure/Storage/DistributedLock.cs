using Azure.Storage.Blobs;
using DragonSpark.Compose;
using Medallion.Threading;
using Medallion.Threading.Azure;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

public class DistributedLock : IDistributedLock
{
	readonly AzureBlobLeaseDistributedLock _lock;

	public DistributedLock(IContainer container, string name) : this(new(container.Get(), name)) {}

	public DistributedLock(AzureBlobLeaseDistributedLock @lock) => _lock = @lock;

	public async ValueTask<IDistributedSynchronizationHandle> Get() => await _lock.AcquireAsync().Off();
}

public class DistributedLock<T> : IDistributedLock<T>
{
	readonly BlobContainerClient _client;
	readonly string              _qualifier;
	readonly Func<T, string>     _identifier;

	public DistributedLock(BlobContainerClient client, string qualifier, Func<T, string> identifier)
	{
		_client     = client;
		_qualifier  = qualifier;
		_identifier = identifier;
	}

	public async ValueTask<IDistributedSynchronizationHandle> Get(T parameter)
	{
		var name  = $"{_qualifier}_{_identifier(parameter)}";
		var @lock = new AzureBlobLeaseDistributedLock(_client, name);
		return await @lock.AcquireAsync().Off();
	}
}