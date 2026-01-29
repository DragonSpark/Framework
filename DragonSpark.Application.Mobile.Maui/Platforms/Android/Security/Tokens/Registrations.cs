using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class Registrations : Commands<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations()
        : base(Application.Security.Tokens.Registrations.Default, LocalRegistrations.Default) {}
}