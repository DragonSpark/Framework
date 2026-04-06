using DragonSpark.Model.Selection;
using Microsoft.Azure.NotificationHubs;

namespace DragonSpark.Server.Mobile.Notifications;

public interface IRegistrationTag : ISelect<RegistrationDescription, string?>;