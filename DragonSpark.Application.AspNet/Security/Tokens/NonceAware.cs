using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Security.Tokens;

public class NonceAware<TIn, TOut> : IStopAware<TIn, TOut>
{
    readonly ValidateCurrentNonce  _validate;
    readonly IStopAware<TIn, TOut> _previous;
    readonly IMarkUsed             _mark;

    protected NonceAware(ValidateCurrentNonce validate, IStopAware<TIn, TOut> previous, IMarkUsed mark)
    {
        _validate = validate;
        _previous = previous;
        _mark     = mark;
    }

    public async ValueTask<TOut> Get(Stop<TIn> parameter)
    {
        var (_, stop) = parameter;
        var token  = await _validate.Off(parameter);
        var result = await _previous.Off(parameter);
        await _mark.Off(new(token, stop));
        return result;
    }
}