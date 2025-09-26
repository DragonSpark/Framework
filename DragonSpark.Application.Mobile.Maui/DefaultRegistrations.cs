using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui;

sealed class DefaultRegistrations : Commands<IServiceCollection>
{
    public static DefaultRegistrations Default { get; } = new();

    DefaultRegistrations()
        : base(Mobile.DefaultRegistrations.Default, LocalRegistrations.Default, Diagnostics.Registrations.Default,
               Device.Camera.Registrations.Default, Runtime.Registrations.Default) {}
}