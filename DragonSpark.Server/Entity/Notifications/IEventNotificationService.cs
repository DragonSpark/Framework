namespace DragonSpark.Application.Communication.Entity.Notifications
{
	public interface IEventNotificationService
	{
		void Notify( EventNotification eventNotification );
	}
}