using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection;

public class Configuring<TSource, TResult> : ISelecting<TSource, TResult>
{
    readonly Await<TSource, TResult> _select;
    readonly Operate<(TSource, TResult)> _configure;

    protected Configuring(Await<TSource, TResult> select, Operate<(TSource, TResult)> configure)
    {
        _select = select;
        _configure = configure;
    }

    public async ValueTask<TResult> Get(TSource parameter)
    {
        var result = await _select(parameter);
        await _configure((parameter, result)).ConfigureAwait(false);
        return result;
    }
}
