using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using Radzen;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Diagnostics;

sealed class NotificationAwareExceptions : IExceptions
{
	readonly IExceptions                 _exceptions;
	readonly Action<NotificationMessage> _notify;
	readonly IExceptionNotification      _message;

	public NotificationAwareExceptions(IExceptions exceptions, Notify notify, IExceptionNotification message)
		: this(exceptions, notify.Then().Protect(), message) {}

	public NotificationAwareExceptions(IExceptions exceptions, Action<NotificationMessage> notify,
	                                   IExceptionNotification message)
	{
		_exceptions = exceptions;
		_notify     = notify;
		_message    = message;
	}

	public ValueTask Get(ExceptionInput parameter)
	{
		var result  = _exceptions.Get(parameter);
		var message = _message.Get(parameter.Exception);
		if (message is not null)
		{
			_notify(message);
		}

		return result;
	}
}