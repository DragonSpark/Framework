using DragonSpark.Application.Diagnostics;
using Radzen;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Diagnostics
{
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

		public ValueTask Get((Type Owner, Exception Exception) parameter)
		{
			var result = _exceptions.Get(parameter);

			_notifications.Notify(_message.Get(parameter.Exception));

			return result;
		}
	}
}