using DragonSpark.Application.Connections.Events;
using DragonSpark.Compose;
using DragonSpark.Model;

namespace DragonSpark.Azure.Events.Receive;

public class SubscriberOf<T> : Subscriber<None>, ISubscriber
{
	protected SubscriberOf() : base(A.Type<T>().FullName.Verify()) {}

	public ISubscription Get(SubscriberInput parameter)
	{
		var (recipient, body) = parameter;
		return Get(new SubscriberInput<None>(recipient, body.Start().Accept()));
	}
}