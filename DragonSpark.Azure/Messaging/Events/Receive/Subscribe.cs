using DragonSpark.Application.Connections.Events;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Events.Receive;

public class Subscribe<T, U> : Subscribe<T> where U : Message<T>
{
	protected Subscribe()
		: this(KeyedEntry<U>.Default.Get(new(A.Type<U>().FullName.Verify())).Handlers, CreateOperation<T, U>.Default) {}

	protected Subscribe(Handlers handlers, ISelect<Func<T, Task>, IOperation<object>> operation)
		: base(handlers, operation) {}
}

public class Subscribe<T> : ISubscribe<T>
{
	readonly Handlers                                   _handlers;
	readonly ISelect<Func<T, Task>, IOperation<object>> _operation;

	protected Subscribe()
		: this(KeyedEntry<T>.Default.Get(new(A.Type<T>().FullName.Verify())).Handlers, CreateOperation<T>.Default) {}

	protected Subscribe(Handlers handlers, ISelect<Func<T, Task>, IOperation<object>> operation)
	{
		_handlers  = handlers;
		_operation = operation;
	}

	public ISubscription Get(Func<T, Task> parameter)
	{
		var operation = _operation.Get(parameter);
		return new Subscription(_handlers, operation);
	}
}