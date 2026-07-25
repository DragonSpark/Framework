using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Application.AspNet.Workers;
using DragonSpark.Compose;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class CreateProcessNotification : IStopAware<DurableMessageProperties>
{
	readonly NewProcessNotification    _notification;
	readonly Save<ProcessNotification> _save;
	readonly Warn                      _warn;

	public CreateProcessNotification(NewProcessNotification notification, Save<ProcessNotification> save, Warn warn)
	{
		_notification = notification;
		_save         = save;
		_warn         = warn;
	}

	public async ValueTask Get(Stop<DurableMessageProperties> parameter)
	{
		var ((identifier, _, destination, _, _), stop) = parameter;
		var notification = await _notification.Off(parameter);
		if (notification is not null)
		{
			await _save.Off(new(notification, stop));
		}
		else
		{
			_warn.Execute(destination, identifier);
		}
	}

	public sealed class Warn : LogWarning<string, Guid?>
	{
		public Warn(ILogger<Warn> logger)
			: base(logger, "A request was made to {Queue} to start {Identity} but this process could not be found") {}
	}
}