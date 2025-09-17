using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Attestation;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Register<PlayStoreVerificationSettings>()
                 //
                 .Start<IAttestationToken>()
                 .Forward<AttestationToken>()
                 .Include(x => x.Dependencies)
                 .Singleton();
    }
}