using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public abstract class ValidationAwareAddPasskey : IAddPasskey
{
    readonly IAddPasskey      _previous;
    readonly IResulting<byte> _count;
    readonly byte             _maximum;

    protected ValidationAwareAddPasskey(IAddPasskey previous, IResulting<byte> count, byte maximum)
    {
        _previous = previous;
        _count    = count;
        _maximum  = maximum;
    }

    public async ValueTask<AddPasskeyResult> Get(Stop<AddPasskeyInput> parameter)
    {
        var ((credential, error), _) = parameter;
        var message = !error.IsNullOrEmpty()
                          ? $"Error: {error}"
                          : credential.IsNullOrEmpty()
                              ? "Error: The browser did not provide a passkey."
                              : await _count.Off() > _maximum
                                  ? "Error: You have reached the maximum number of allowed passkeys."
                                  : null;
        return message is not null ? new FailedAddPasskeyResult(message) : await _previous.Off(parameter);
    }
}