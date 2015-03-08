using DragonSpark.Objects;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel.DomainServices.Server;

namespace DragonSpark.Application.Communication.Entity.Notifications
{
	public abstract class AuthenticationServiceBase<TStorage, TUser> : Security.AuthenticationServiceBase<TStorage, TUser> where TStorage : DbContext, new() where TUser : ApplicationUser, new()
	{
		protected override void Process( TUser result )
		{
			base.Process( result );
			DbContext.Entry( result ).Collection( y => y.Notifications ).Query().Where( y => y.IsActive ).Load();
			result.HasNotificationAlert = result.Notifications.Any( y => y.IsActive && ( !result.LastNotificationUpdate.HasValue || y.Created > result.LastNotificationUpdate ) );
		}
	}

    public class ApplicationUser : Security.ApplicationUser
    {
        public string EmailAddress { get; set; }

        [DefaultPropertyValue( true )]
        public bool EnableEmailNotifications { get; set; }

        [Display( AutoGenerateField = false )]
        public DateTime? LastNotificationUpdate { get; set; }

        public bool HasNotificationAlert { get; set; }

        [Include, Association( "NotificationUser", "Name", "UserName" )]
        public virtual Collection<Notification> Notifications
        {
            get { return notifications; }
        }	readonly Collection<Notification> notifications = new Collection<Notification>();
    }
}