using DragonSpark.Application.Diagnostics;
using Radzen;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Diagnostics;

sealed class NotificationAwareExceptions : IExceptions
{
	readonly IExceptions            _exceptions;
	readonly NotificationService    _notifications;
	readonly IExceptionNotification _message;

	public NotificationAwareExceptions(IExceptions exceptions, NotificationService notifications,
	                                   IExceptionNotification message)
	{
		_exceptions    = exceptions;
		_notifications = notifications;
		_message       = message;
	}

	public ValueTask Get(ExceptionInput parameter)
	{
		var result = _exceptions.Get(parameter);

		var message = _message.Get(parameter.Exception);
		if (message is not null)
		{
			_notifications.Notify(message);
		}

		return result;
	}
}