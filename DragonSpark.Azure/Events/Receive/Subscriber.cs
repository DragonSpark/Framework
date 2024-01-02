using DragonSpark.Application.Connections.Events;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events.Receive;

public class Subscriber<T, U> : ISubscriber<T> where U : Message<T>
{
	readonly IKeyedEntry                                _entry;
	readonly string                                     _key;
	readonly ISelect<Func<T, Task>, IOperation<object>> _operation;

	protected Subscriber()
		: this(KeyedEntry<U>.Default, A.Type<U>().FullName.Verify(), CreateOperation<T, U>.Default) {}

	protected Subscriber(IKeyedEntry entry, string key, ISelect<Func<T, Task>, IOperation<object>> operation)
	{
		_entry     = entry;
		_operation = operation;
		_key       = key;
	}

	public ISubscription Get(SubscriberInput<T> parameter)
	{
		var (recipient, subject) = parameter;
		var (_, handlers)        = _entry.Get(new(recipient, _key));
		var operation = _operation.Get(subject);
		return new Subscription(handlers, operation);
	}
}

public class Subscriber<T> : ISubscriber<T>
{
	readonly IKeyedEntry                                _entry;
	readonly ISelect<Func<T, Task>, IOperation<object>> _operation;
	readonly string                                     _key;

	protected Subscriber() : this(A.Type<T>().FullName.Verify()) {}

	protected Subscriber(string key) : this(KeyedEntry<T>.Default, key, CreateOperation<T>.Default) {}

	protected Subscriber(IKeyedEntry entry, string key, ISelect<Func<T, Task>, IOperation<object>> operation)
	{
		_entry     = entry;
		_operation = operation;
		_key       = key;
	}

	public ISubscription Get(SubscriberInput<T> parameter)
	{
		var (recipient, subject) = parameter;
		var (_, handlers)        = _entry.Get(new(recipient, _key));
		var operation = _operation.Get(subject);
		return new Subscription(handlers, operation);
	}
}