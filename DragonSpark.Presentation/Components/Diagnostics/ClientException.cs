using Radzen;
using System;

namespace DragonSpark.Presentation.Components.Diagnostics
{
	public class ClientException : InvalidOperationException
	{
		public ClientException(string? message, NotificationSeverity severity = NotificationSeverity.Error)
			: this("There was a problem.", message, TimeSpan.FromMilliseconds(5000), severity) {}

		// ReSharper disable once TooManyDependencies
		public ClientException(string summary, string? message, TimeSpan duration,
		                       NotificationSeverity severity = NotificationSeverity.Error) : base(message)
		{
			Summary  = summary;
			Duration = duration;
			Severity = severity;
		}

		public string Summary { get; }
		public TimeSpan Duration { get; }

		public NotificationSeverity Severity { get; }
	}
}