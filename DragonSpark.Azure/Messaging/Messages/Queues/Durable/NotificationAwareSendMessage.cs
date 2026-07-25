using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Workers;
using DragonSpark.Compose;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class NotificationAwareSendMessage : ISendMessage
{
	readonly ISendMessage _previous;
	readonly IScopes      _scopes;

	public NotificationAwareSendMessage(ISendMessage previous, IScopes scopes)
	{
		_previous = previous;
		_scopes   = scopes;
	}

	public async ValueTask Get(Stop<DurableMessageProperties> parameter)
	{
		await _previous.Off(parameter);

		var ((identifier, _, _, _, _), stop) = parameter;
		using var scope = _scopes.Get();
		await scope.Owner.Set<ProcessNotification>()
		           .Where(x => x.Id == identifier)
		           .ExecuteDeleteAsync(stop)
		           .Off();
	}
}