using System;
using System.Threading.Tasks;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications;

public interface INotifications
{
	event EventHandler NotificationReceived;
	Task SendNotification(Stop<NotificationInput> notification);
	void ReceiveNotification(string title, string message);
}