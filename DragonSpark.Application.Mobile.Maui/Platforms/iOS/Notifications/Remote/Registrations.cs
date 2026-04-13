using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

sealed class Registrations : Commands<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations()
        : base(Device.Notifications.Remote.Registrations.Default, LocalRegistrations.Default) {}
}