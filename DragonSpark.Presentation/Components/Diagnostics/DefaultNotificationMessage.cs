using Radzen;

namespace DragonSpark.Presentation.Components.Diagnostics
{
	sealed class DefaultNotificationMessage : DragonSpark.Model.Results.Instance<NotificationMessage>
	{
		public static DefaultNotificationMessage Default { get; } = new DefaultNotificationMessage();

		DefaultNotificationMessage() : base(new NotificationMessage
		{
			Severity = NotificationSeverity.Warning,
			Summary  = "There was a problem",
			Detail =
				"A problem was encountered while performing this operation and has been logged for system administrators.",
			Duration = 4000
		}) {}
	}
}