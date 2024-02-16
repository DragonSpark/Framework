using DragonSpark.Application.Connections.Events;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

sealed class Subscription : ISubscription
{
	readonly Handlers           _handlers;
	readonly IOperation<object> _subject;

	public Subscription(Handlers handlers, IOperation<object> subject)
	{
		_handlers = handlers;
		_subject  = subject;
	}

	public ValueTask Get()
	{
		if (!_handlers.Contains(_subject))
		{
			_handlers.Add(_subject);
		}

		return ValueTask.CompletedTask;
	}

	public ValueTask DisposeAsync()
	{
		_handlers.Remove(_subject);
		return ValueTask.CompletedTask;
	}
}