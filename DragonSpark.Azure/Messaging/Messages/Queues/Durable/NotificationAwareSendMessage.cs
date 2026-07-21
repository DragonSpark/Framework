using System.Linq;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Workers;
using DragonSpark.Compose;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class NotificationAwareSendMessage : ISendMessage
{
	readonly ISendMessage _previous;
	readonly IScopes      _scopes;
	readonly ITime        _time;

	public NotificationAwareSendMessage(ISendMessage previous, IScopes scopes) : this(previous, scopes, Time.Default) {}

	public NotificationAwareSendMessage(ISendMessage previous, IScopes scopes, ITime time)
	{
		_previous = previous;
		_scopes   = scopes;
		_time     = time;
	}

	public async ValueTask Get(Stop<DurableMessageProperties> parameter)
	{
		var ((identifier, _, _, _, _), stop) = parameter;
		await _previous.Off(parameter);
		using var scope = _scopes.Get();
		var       time  = _time.Get();
		await scope.Owner.Set<ProcessNotification>()
		           .Where(x => x.Id == identifier && x.Sent == null)
		           .ExecuteUpdateAsync(s => s.SetProperty(x => x.Sent, time), stop)
		           .Off();
	}
}