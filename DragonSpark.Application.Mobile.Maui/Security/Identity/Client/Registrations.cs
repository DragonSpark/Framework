using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity.Client;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.TryDecorate<IClientKeyHash, ClientKeyHash>();
        parameter.TryDecorate<IClearClientKey, ClientKeyHashAwareClearClientKey>();
    }
}