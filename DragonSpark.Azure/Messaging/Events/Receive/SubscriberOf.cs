using DragonSpark.Application.Connections.Events;
using DragonSpark.Compose;

namespace DragonSpark.Azure.Messaging.Events.Receive;

public class SubscriberOf<T> : Subscriber<T>, ISubscriber
{
	protected SubscriberOf() : base(A.Type<T>().FullName.Verify()) {}

	public ISubscription Get(SubscriberInput parameter)
	{
		var (recipient, body) = parameter;
		return Get(new SubscriberInput<T>(recipient, body.Start().Accept<T>()));
	}
}