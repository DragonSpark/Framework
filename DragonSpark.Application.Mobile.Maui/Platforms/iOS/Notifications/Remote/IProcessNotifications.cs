using DragonSpark.Model.Commands;
using Foundation;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

public interface IProcessNotifications : ICommand<NSDictionary>;