using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using Foundation;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Attestation;

sealed class StateAwareAttestationToken : IAttestationToken
{
    readonly IAttestationToken    _previous;
    readonly ClearValidationState _clear;
    readonly Switch               _attempt;

    public StateAwareAttestationToken(IAttestationToken previous, ClearValidationState clear)
        : this(previous, clear, false) {}

    public StateAwareAttestationToken(IAttestationToken previous, ClearValidationState clear, Switch attempt)
    {
        _previous = previous;
        _clear    = clear;
        _attempt  = attempt;
    }

    public async ValueTask<string> Get(Stop<string> parameter)
    {
        try
        {
            return await _previous.Off(parameter);
        }
        catch (NSErrorException) when (_attempt.Up())
        {
            await _clear.Off(parameter);
            return await _previous.Off(parameter);
        }
    }
}