using Android.Content;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

public interface IProcessNotifications : ICommand<Intent>;