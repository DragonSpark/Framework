using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
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

// TODO

sealed class AssertionToken : IAssertionToken
{
    public static AssertionToken Default { get; } = new();

    AssertionToken() {}

    public ValueTask<string> Get(Stop<string> parameter) => string.Empty.ToOperation();
}