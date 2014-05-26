namespace DragonSpark.Application.Communication.Entity.Notifications
{
    public class NullEventNotificationService : IEventNotificationService
    {
        public void Notify( EventNotification eventNotification )
        {}
    }
}