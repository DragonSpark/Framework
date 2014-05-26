using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Objects.Synchronization;

namespace DragonSpark.Application.Communication.Entity.Notifications
{
	[Singleton( typeof(INotificationMessageSender) )]
	public class NotificationMessageSender : INotificationMessageSender
	{
		readonly IApplicationRequestProvider requestProvider;
		readonly IUserNotificationService userNotificationService;

		public NotificationMessageSender( IApplicationRequestProvider requestProvider, IUserNotificationService userNotificationService )
		{
			this.requestProvider = requestProvider;
			this.userNotificationService = userNotificationService;
		}

		public void Send( NotificationMessage notificationMessage )
		{
			var request = new ApplicationRequest { Message = notificationMessage.Body };
			notificationMessage.RequestNotification.RequestIds = notificationMessage.To.Select( x => requestProvider.Add( x.Name, request ) ).ToArray();

			notificationMessage.From.NotNull( x => notificationMessage.RequestNotification.From.SynchronizeFrom( x ) );
			notificationMessage.To.Apply( x => userNotificationService.Notify( x, notificationMessage.RequestNotification, notificationMessage.ReplacementTarget ) );
		}
	}
}