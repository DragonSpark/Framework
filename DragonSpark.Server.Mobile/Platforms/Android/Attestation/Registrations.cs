using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Server.Mobile.Platforms.Android.Attestation.Records;
using Google.Apis.PlayIntegrity.v1;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation;

public sealed class Registrations<T> : ICommand<IServiceCollection> where T : class, IVerificationRecord
{
    public static Registrations<T> Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Register<AndroidPackageSettings>()
                 //
                 .Start<V1Resource>()
                 .Use<ComposeIntegrityService>()
                 .Singleton()
                 //
                 .Then.Start<IProcessIntegrityToken>()
                 .Forward<ProcessIntegrityToken>()
                 .Singleton()
                 //
                 .Then.Start<IValidVerification>()
                 .Forward<ValidVerification>()
                 .Singleton()
                 //
                 .Then.Start<INewAttestation>()
                 .Forward<NewAttestation<T>>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.Start<IExistingAttestation>()
                 .Forward<ExistingAttestation<T>>()
                 .Include(x => x.Dependencies)
                 .Singleton()
            ;
    }
}

/*public readonly record struct VerificationInput(Guid? Identity, string KeyHash, string Challenge, string Input);*/
/*public interface ILoadAttestation : IStopAware<VerificationInput, IVerificationRecord?>;

sealed class LoadAttestation<T> : ILoadAttestation where T : class, IVerificationRecord
{
    readonly IVerificationRecord<T> _record;

    public LoadAttestation(IVerificationRecord<T> record) => _record = record;

    public async ValueTask<IVerificationRecord?> Get(Stop<VerificationInput> parameter) => await _record.Off(parameter);
}*/