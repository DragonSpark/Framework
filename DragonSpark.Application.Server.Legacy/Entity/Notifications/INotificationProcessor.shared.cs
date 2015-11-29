namespace DragonSpark.Application.Communication.Entity.Notifications
{
	public interface INotificationProcessor
	{
		void Process( INotification notification );
	}
}