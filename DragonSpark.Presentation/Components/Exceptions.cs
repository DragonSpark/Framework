using Microsoft.Extensions.Logging;
using Radzen;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	public sealed class Exceptions : IExceptions
	{
		readonly ILoggerFactory      _factory;
		readonly NotificationService _notifications;

		public Exceptions(ILoggerFactory factory, NotificationService notifications)
		{
			_factory       = factory;
			_notifications = notifications;
		}

		public ValueTask Get((Type Owner, Exception Exception) parameter)
		{
			var (owner, exception) = parameter;

			_factory.CreateLogger(owner)
			        .LogError(exception, "A problem was encountered while performing this operation.");

			_notifications.Notify(new NotificationMessage
			{
				Severity = NotificationSeverity.Warning,
				Summary  = "There was a problem",
				Detail   = "A problem was encountered while performing this operation and has been logged for system administrators.",
				Duration = 4000
			});
			return new ValueTask(Task.CompletedTask);
		}
	}
}