using DragonSpark.Compose;

namespace DragonSpark.Application.AspNet.Security.Tokens;

public class ValidNonceAwareResult<T> : DragonSpark.Model.Operations.Results.Stop.IStopAware<T>
{
    readonly ValidateCurrentNonce                                    _validate;
    readonly DragonSpark.Model.Operations.Results.Stop.IStopAware<T> _previous;

    public ValidNonceAwareResult(ValidateCurrentNonce validate,
                                 DragonSpark.Model.Operations.Results.Stop.IStopAware<T> previous)
    {
        _validate = validate;
        _previous = previous;
    }

    public async ValueTask<T> Get(CancellationToken parameter)
    {
        await _validate.Off(parameter);
        return await _previous.Off(parameter);
    }
}