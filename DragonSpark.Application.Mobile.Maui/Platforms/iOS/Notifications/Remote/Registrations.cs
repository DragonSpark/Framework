using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

sealed class Registrations : Commands<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations()
        : base(Device.Notifications.Remote.Registrations.Default, 
               Device.Notifications.Remote.Messages.Registrations.Default, LocalRegistrations.Default) {}
}