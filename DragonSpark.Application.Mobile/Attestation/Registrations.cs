using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Attestation;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IClientKeyHash>()
                 .Forward<ClientKeyHash>()
                 .Singleton()
                 //
                 .Then.Start<IAttest>()
                 .Forward<Attest>()
                 .Singleton();
    }
}