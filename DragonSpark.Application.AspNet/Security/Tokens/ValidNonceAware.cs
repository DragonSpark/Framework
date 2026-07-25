using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Security.Tokens;

public class ValidNonceAware<TIn, TOut> : IStopAware<TIn, TOut>
{
    readonly ValidateCurrentNonce  _validate;
    readonly IStopAware<TIn, TOut> _previous;

    public ValidNonceAware(ValidateCurrentNonce validate, IStopAware<TIn, TOut> previous)
    {
        _validate = validate;
        _previous = previous;
    }

    public async ValueTask<TOut> Get(Stop<TIn> parameter)
    {
        await _validate.Off(parameter);
        return await _previous.Off(parameter);
    }
}