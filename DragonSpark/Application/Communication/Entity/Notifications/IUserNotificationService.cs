using DragonSpark.Application.Communication.Security;

namespace DragonSpark.Application.Communication.Entity.Notifications
{
	public interface IUserNotificationService
	{
		void Notify( ApplicationUser user, Notification notification, params ApplicationUser[] replacements );
	}
}