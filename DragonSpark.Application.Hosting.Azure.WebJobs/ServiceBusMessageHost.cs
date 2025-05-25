using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Stop;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public class ServiceBusMessageHost : IAllocated<ServiceBusReceivedMessage>
{
	readonly IAllocatedStopAware<string> _previous;
	readonly WebJobsShutdownWatcher      _watcher;

	protected ServiceBusMessageHost(IStopAware<Guid> operation, ILoggerFactory factory)
		: this(new LoggedParsedIdentityTextHost(operation, factory), new()) {}

	public ServiceBusMessageHost(IAllocatedStopAware<string> previous, WebJobsShutdownWatcher watcher)
	{
		_previous = previous;
		_watcher  = watcher;
	}

	public virtual Task Get(ServiceBusReceivedMessage parameter)
		=> _previous.Get(new(parameter.Body.ToString(), _watcher.Token));
}