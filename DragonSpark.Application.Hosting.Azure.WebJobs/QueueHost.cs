using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public class QueueHost : IAllocated<string>
{
	readonly IAllocated<string> _queue;

	protected QueueHost(IOperation<Guid> operation) : this(new QueueApplication(operation)) {}

	protected QueueHost(IAllocated<string> queue) => _queue = queue;

	public virtual Task Get(string parameter) => _queue.Get(parameter);
}