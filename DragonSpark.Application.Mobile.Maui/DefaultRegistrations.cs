using DragonSpark.Application.Mobile.Maui.Security.Identity.Client;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Maui;

sealed class DefaultRegistrations : Commands<IServiceCollection>
{
    public static DefaultRegistrations Default { get; } = new();

    DefaultRegistrations()
        : base(Mobile.DefaultRegistrations.Default, LocalRegistrations.Default, Diagnostics.Registrations.Default,
               Device.Camera.Registrations.Default, Device.Security.Registrations.Default.Deferred(),
               Device.Security.Passkey.Registrations.Default, Runtime.Registrations.Default,
               Registrations.Default.Deferred()) {}
}