namespace DragonSpark.Application.Communication.Entity.Notifications
{
	public interface INotificationProcessorLocator
	{
		INotificationProcessor Locate( INotification applicationRequest );
	}
}