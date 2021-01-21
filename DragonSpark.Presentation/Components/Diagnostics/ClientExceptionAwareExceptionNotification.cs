using Radzen;
using System;

namespace DragonSpark.Presentation.Components.Diagnostics
{
	sealed class ClientExceptionAwareExceptionNotification : IExceptionNotification
	{
		readonly IExceptionNotification _previous;

		public ClientExceptionAwareExceptionNotification(IExceptionNotification previous) => _previous = previous;

		public NotificationMessage Get(Exception parameter)
			=> parameter is ClientException client
				   ? new NotificationMessage
				   {
					   Summary  = client.Summary,
					   Severity = client.Severity,
					   Duration = client.Duration.TotalMilliseconds,
					   Detail   = client.Message
				   }
				   : _previous.Get(parameter);
	}
}