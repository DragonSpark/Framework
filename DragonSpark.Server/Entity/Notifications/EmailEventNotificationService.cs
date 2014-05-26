using System;
using System.Net;
using System.Net.Mail;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Communication.Entity.Notifications
{
    public class EmailEventNotificationService : IEventNotificationService, IDisposable
	{
		readonly EmailEventNotificationSettings settings;
		readonly string template;
		readonly SmtpClient client;

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed in .Dispose()." )]
        public EmailEventNotificationService( EmailEventNotificationSettings settings, string template )
		{
			this.settings = settings;
			this.template = template;
			client = new SmtpClient( settings.SmtpServerHost, settings.ServerPort ) { Credentials = new NetworkCredential( settings.UserName, settings.Password ), EnableSsl = true, DeliveryMethod = SmtpDeliveryMethod.Network };
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope" )]
        public void Notify( EventNotification eventNotification )
		{
			var address = eventNotification.ToAddress ?? new MailAddress( settings.ToAddress );
			var body = string.Format( NamedTokenFormatter.Instance, template, new { Settings = settings, Notification = eventNotification, Email = address.Address } );
            using ( var message = new MailMessage( settings.FromAddress, address.ToString(), eventNotification.Subject, body ) { IsBodyHtml = true } )
            {
                DragonSpark.Runtime.Logging.Try( () => client.Send( message ) );
            }

		}

        public void Dispose() 
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing )
		{
            client.Dispose();
        }
	}
}