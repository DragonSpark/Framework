using DragonSpark.Extensions;

namespace DragonSpark.Application.Communication.Entity.Notifications
{
	public abstract class NotificationProcessorBase<TRequest> : INotificationProcessor where TRequest : class, INotification
	{
		void INotificationProcessor.Process( INotification applicationRequest )
		{
			applicationRequest.As<TRequest>( ProcessRequest );
		}

		protected abstract void ProcessRequest( TRequest notification );
	}
}