using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Mobile.Maui.Platforms.iOS.Attestation.Assertion;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Attestation;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IAttestationToken>()
                 .Forward<AttestationToken>()
                 .Singleton()
                 //
                 .Then.Start<IAssertionToken>()
                 .Forward<AssertionToken>()
                 .Singleton()
                 //
                 .Then.Start<IClientKey>()
                 .Forward<ClientKey>()
                 .Singleton()
                 //
                 .Then.Start<IClearClientKey>()
                 .Forward<ClearClientKey>()
                 .Singleton();
    }
}