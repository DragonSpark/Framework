using Radzen;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	sealed class NotificationAwareExceptions : IExceptions
	{
		readonly IExceptions         _exceptions;
		readonly NotificationService _notifications;
		readonly NotificationMessage _message;

		public NotificationAwareExceptions(IExceptions exceptions, NotificationService notifications)
			: this(exceptions, notifications, DefaultNotificationMessage.Default) {}

		public NotificationAwareExceptions(IExceptions exceptions, NotificationService notifications,
		                                   NotificationMessage message)
		{
			_exceptions    = exceptions;
			_notifications = notifications;
			_message       = message;
		}

		public ValueTask Get((Type Owner, Exception Exception) parameter)
		{
			var result = _exceptions.Get(parameter);

			_notifications.Notify(_message);

			return result;
		}
	}
}