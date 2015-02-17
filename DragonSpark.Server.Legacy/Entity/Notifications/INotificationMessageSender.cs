namespace DragonSpark.Application.Communication.Entity.Notifications
{
    public interface INotificationMessageSender
	{
		void Send( NotificationMessage notificationMessage );
	}
}