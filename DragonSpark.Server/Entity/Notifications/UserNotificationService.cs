using System;
using System.Net.Mail;
using System.Web;
using DragonSpark.Extensions;
using DragonSpark.IoC;

namespace DragonSpark.Application.Communication.Entity.Notifications
{
    [Singleton( typeof(IUserNotificationService) )]
	class UserNotificationService : IUserNotificationService
	{
		readonly IEventNotificationService notificationService;

		public UserNotificationService( IEventNotificationService notificationService )
		{
			this.notificationService = notificationService;
		}

		public void Notify( ApplicationUser user, Notification notification, params ApplicationUser[] replacements )
		{
			// Add notification:
			user.NotNull( x => x.Notifications.Add( notification ) );
			
			// Send Email:                                                     		
			var email = notification.SendEmail && user.Transform( x => x.EnableEmailNotifications, () => true );
			email.IsTrue( () =>
			{
			    var message = notification.Message;
			    // user.ToEnumerable( replacements ).NotNull().Apply( x => message = message.Replace( x.GetFullName(), x.GetNamedLink() ) ); // TODO: Employ a templating system to perform replacements.

			    var eventNotification = new EventNotification
			                                {
			                                    Subject = notification.Title, 
			                                    ToAddress = user.Transform( x => new MailAddress( x.EmailAddress, x.DisplayName ) ), 
			                                    Message = message, 
			                                    ViewLocation = new Uri( string.Format( "http://{0}/", HttpContext.Current.Request.Url.Host ) )
			                                };
			    notificationService.Notify( eventNotification );
			} );
		}
	}
}