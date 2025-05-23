using DragonSpark.Application.Connections.Events;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public class Subscribe<T, U> : Subscribe<T> where U : Message<T>
{
	protected Subscribe()
		: this(KeyedEntry<U>.Default.Get(new(A.Type<U>().FullName.Verify())).Handlers, CreateOperation<T, U>.Default) {}

	protected Subscribe(Handlers handlers, ISelect<Func<Stop<T>, Task>, IStopAware<object>> operation)
		: base(handlers, operation) {}
}

public class Subscribe<T> : ISubscribe<T>
{
	readonly Handlers                                         _handlers;
	readonly ISelect<Func<Stop<T>, Task>, IStopAware<object>> _operation;

	protected Subscribe()
		: this(KeyedEntry<T>.Default.Get(new(A.Type<T>().FullName.Verify())).Handlers, CreateOperation<T>.Default) {}

	protected Subscribe(Handlers handlers, ISelect<Func<Stop<T>, Task>, IStopAware<object>> operation)
	{
		_handlers  = handlers;
		_operation = operation;
	}

	public ISubscription Get(Func<Stop<T>, Task> parameter)
	{
		var operation = _operation.Get(parameter);
		return new Subscription(_handlers, operation);
	}
}