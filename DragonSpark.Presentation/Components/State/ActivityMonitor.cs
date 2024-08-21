using DragonSpark.Presentation.Components.Eventing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

sealed class ActivityMonitor : IActivityMonitor
{
	readonly IPublisher<ActivityUpdatedMessage> _publisher;
	readonly HashSet<IActivityReceiver>         _receivers;

	public ActivityMonitor(IPublisher<ActivityUpdatedMessage> publisher) : this(publisher, new()) {}

	public ActivityMonitor(IPublisher<ActivityUpdatedMessage> publisher, HashSet<IActivityReceiver> receivers)
	{
		_publisher = publisher;
		_receivers = receivers;
	}

	public void Execute(IActivityReceiver parameter)
	{
		_receivers.Add(parameter);
	}

	public ValueTask Get() => _publisher.Get(new(this));

	public bool Active => _receivers.Any(x => x.Active);
}