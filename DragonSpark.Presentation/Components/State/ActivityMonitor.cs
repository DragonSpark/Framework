using DragonSpark.Model.Sequences.Collections;
using DragonSpark.Presentation.Components.Eventing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

sealed class ActivityMonitor : Membership<IActivityReceiver>, IActivityMonitor
{
	readonly IPublisher<ActivityUpdatedMessage> _publisher;
	readonly HashSet<IActivityReceiver>         _receivers;

	public ActivityMonitor(IPublisher<ActivityUpdatedMessage> publisher) : this(publisher, new()) {}

	public ActivityMonitor(IPublisher<ActivityUpdatedMessage> publisher, HashSet<IActivityReceiver> receivers)
		: base(receivers)
	{
		_publisher = publisher;
		_receivers = receivers;
	}

	public ValueTask Get() => _publisher.Get(new(this));

	public bool Active => _receivers.Any(x => x.Active);
}