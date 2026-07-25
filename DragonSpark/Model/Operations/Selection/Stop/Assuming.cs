using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations.Selection.Stop;

public class Assuming<TIn, TOut> : IStopAware<TIn, TOut>
{
    readonly IStopAware<ISelect<TIn, TOut>> _previous;

    protected Assuming(Func<ISelect<TIn, TOut>> previous) : this(previous.Start().Operation().Out().AsStop()) {}

    protected Assuming(IStopAware<ISelect<TIn, TOut>> previous) => _previous = previous;

    public async ValueTask<TOut> Get(Stop<TIn> parameter)
    {
        var previous = await _previous.Off(parameter);
        return previous.Get(parameter);
    }
}