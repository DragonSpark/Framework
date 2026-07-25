using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Operations.Selection.Stop.Conditions;

public class Assuming<TIn, TOut> : IStopAware<TIn, TOut>
{
    readonly IStopAware<IConditional<TIn, TOut>> _previous;

    protected Assuming(Func<IConditional<TIn, TOut>> previous) : this(previous.Start().Operation().Out().AsStop()) {}

    protected Assuming(IStopAware<IConditional<TIn, TOut>> previous) => _previous = previous;

    public async ValueTask<TOut> Get(Stop<TIn> parameter)
    {
        var previous = await _previous.Off(parameter);
        return previous.Get(parameter);
    }
}