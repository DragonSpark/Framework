using System;
using System.Threading.Tasks;
using DragonSpark.Model.Operations.Allocated;

namespace DragonSpark.Application.Mobile.Device.Notifications;

public interface INotifications
{
	event EventHandler NotificationReceived;
	Task SendNotification(Token<NotificationInput> notification);
	void ReceiveNotification(string title, string message);
}