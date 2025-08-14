using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Attestation;

public class Attestation<T> : IAttestation<T>
{
    readonly IStopAware<string>               _challenge;
    readonly IAltering<string>                _attestation;
    readonly IStopAware<VerificationInput, T> _result;

    protected Attestation(IStopAware<string> challenge, IStopAware<VerificationInput, T> result)
        : this(challenge, AttestationToken.Default, result) {}

    protected Attestation(IStopAware<string> challenge, IAltering<string> attestation,
                          IStopAware<VerificationInput, T> result)
    {
        _challenge   = challenge;
        _attestation = attestation;
        _result      = result;
    }

    public async ValueTask<T> Get(CancellationToken parameter)
    {
        var challenge   = await _challenge.Off(parameter);
        var attestation = await _attestation.Off(challenge.Stop(parameter));
        var result      = await _result.Off(new(new(challenge, attestation), parameter));
        return result;
    }
}